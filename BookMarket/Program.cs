using BookMarket.Services.Entities;
using BookMarket.Services.Telnet;
using DnsClient;
using Entities.DateBase;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddTransient<TcpClient>()
    .AddTransient<ILookupClient, LookupClient>();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

Task.Run(() => new TelnetCommandsService());

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

using (ApplicationContext db = new ApplicationContext())
{

    // создаем два объекта User
    Product product1 = new Product { Author = "Tom", Title = "B1", YearPublication = 33, Quantity = 5 };
    Product product2 = new Product { Author = "Alice", Title = "B2", YearPublication = 26, Quantity = 1 };

    // добавляем их в бд
    db.Products.AddRange(product1, product2);
    db.SaveChanges();
}
// получение данных
using (ApplicationContext db = new ApplicationContext())
{
    // получаем объекты из бд и выводим на консоль
    var products = db.Products.ToList();
    Console.WriteLine("Products list:");
    foreach (Product p in products)
    {
        Console.WriteLine($"{p.Id}.{p.Author} - {p.Title} - {p.YearPublication} - {p.Quantity}");
    }
}

app.Run();
