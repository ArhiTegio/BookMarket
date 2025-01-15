using BookMarket.Extansions;
using BookMarket.Services.Entities.Base;
using Entities.DateBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookMarket.Services.Entities
{
    public partial class ApplicationContext : DbContextBase
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationContext(DbContextOptions<DbContextBase> options) : base(options)
        {
#if DEBUG
            //foreach(var e in Products)
            //{
            //    Products.Remove(e);
            //}
            //SaveChanges();

            if (Products.Count() == 0)
            {

                var rnd = new Random();
                var count = rnd.Next(20, 100);
                for (int i = 0; i < count; ++i)
                {
                    var product = new Product()
                    {
                        Id = i,
                        Author = "".LoremIpsum(3, 3, 1, 1, 1, CapitalType.All),
                        Title = "".LoremIpsum(2, 7, 1, 1, 1, CapitalType.First),
                        Quantity = rnd.Next(1, 10),
                        YearPublication = rnd.Next(DateTime.Now.Year - 20, DateTime.Now.Year),
                    };
                    Products.Add(product);
                }
                SaveChanges();
            }
#endif
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            configuration.GetConnectionString("DefaultConnection");

            Npgsql.NpgsqlConnection.ClearPool(new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")) as Npgsql.NpgsqlConnection);
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(f => f.Id)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.YearPublication)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .IsUnicode(false);
            });
        }
    }
}
