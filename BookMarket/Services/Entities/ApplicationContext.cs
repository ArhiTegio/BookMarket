using BookMarket.Services.Entities.Base;
using Entities.DateBase;
using Microsoft.EntityFrameworkCore;

namespace BookMarket.Services.Entities
{
    public class ApplicationContext : DbContextBase
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres");
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
