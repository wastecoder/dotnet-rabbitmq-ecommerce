using Microsoft.EntityFrameworkCore;
using SalesService.Api.Domain.Entities;

namespace SalesService.Api.Infrastructure.Database;

public class SalesDbContext(DbContextOptions<SalesDbContext> options)
    : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureOrder(modelBuilder);
        ConfigureOrderItem(modelBuilder);
    }

    private static void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(x => x.Id);

            // Indexes
            e.HasIndex(x => x.Status);

            // Properties
            e.Property(x => x.Status)
                .IsRequired();

            e.Property(x => x.TotalAmount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            e.Property(x => x.Notes)
                .HasMaxLength(1000);

            e.Property(x => x.CreatedAt)
                .IsRequired();

            e.Property(x => x.UpdatedAt)
                .IsRequired();

            e.Property(x => x.IsDeleted)
                .IsRequired();

            e.Property(x => x.DeletedAt)
                .IsRequired(false);

            // Relationships
            e.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureOrderItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(x => x.Id);

            // FK
            e.Property(x => x.OrderId)
                .IsRequired();

            // Product info
            e.Property(x => x.ProductId)
                .IsRequired();

            e.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(x => x.UnitPrice)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            e.Property(x => x.Quantity)
                .IsRequired();

            // TotalPrice: Shadow property
            e.Ignore(x => x.TotalPrice);

            // TotalPrice: Computed Column
            // e.Property(x => x.TotalPrice)
            //     .HasComputedColumnSql(@"(""UnitPrice"" * ""Quantity"")", stored: true);
        });
    }
}