using BookMarket.Services.Entities;
using BookMarket.Services.Entities.Base;
using BookMarket.Services.Telnet;
using DnsClient;
using Entities.DateBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

var builder = WebApplication
    .CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddTransient<TcpClient>()
    .AddTransient<ILookupClient, LookupClient>();


builder.Services.AddDbContextPool<ApplicationContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
    {
        options.SetPostgresVersion(new Version("9.6"));
        options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), new string[] { "57P01" });
        options.MigrationsAssembly("Initial");
    }));

// Add services to the container.
builder.Services.AddRazorPages();
var app = builder.Build();

var optionsBuilder = new DbContextOptionsBuilder<DbContextBase>();
var options = optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
{
    options.SetPostgresVersion(new Version("9.6"));
    options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), new string[] { "57P01" });
    options.MigrationsAssembly("Initial");
}).Options;

Task.Run(() => { new TelnetCommandsService(options); });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();
