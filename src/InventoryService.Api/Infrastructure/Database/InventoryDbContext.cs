using InventoryService.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Infrastructure.Database;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);

            e.HasIndex(x => x.Name);

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(x => x.Description)
                .HasMaxLength(1000);

            e.Property(x => x.Price)
                .HasColumnType("decimal(10,2)");

            e.Property(x => x.Quantity)
                .IsRequired();

            e.Property(x => x.CreatedAt)
                .IsRequired();

            e.Property(x => x.UpdatedAt)
                .IsRequired();
        });
    }
}