using Gateway.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<UserAccount> Users => Set<UserAccount>();

    public DbSet<ProductMetrics> ProductMetrics => Set<ProductMetrics>();
    public DbSet<ProductSalesStats> ProductSalesStats => Set<ProductSalesStats>();
    public DbSet<StockAlert> StockAlerts => Set<StockAlert>();
    public DbSet<SalesMetrics> SalesMetrics => Set<SalesMetrics>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUserAccount(modelBuilder);
        ConfigureProductMetrics(modelBuilder);
        ConfigureProductSalesStats(modelBuilder);
        ConfigureStockAlert(modelBuilder);
        ConfigureSalesMetrics(modelBuilder);
    }

    private static void ConfigureUserAccount(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();

            e.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(300);

            e.Property(x => x.FullName)
                .HasMaxLength(100);

            e.Property(x => x.Role).IsRequired();
        });
    }


    private static void ConfigureProductMetrics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductMetrics>(e =>
        {
            e.HasKey(x => x.ProductId);
            e.HasIndex(x => x.Quantity);
            
            e.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(x => x.Quantity).IsRequired();
            e.Property(x => x.LowStockThreshold).IsRequired();
            e.Property(x => x.LastUpdated).IsRequired();
        });
    }


    private static void ConfigureProductSalesStats(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductSalesStats>(e =>
        {
            e.HasKey(x => x.ProductId);
            e.HasIndex(x => x.TotalSales);

            e.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            e.Property(x => x.TotalSales).IsRequired();
            e.Property(x => x.LastUpdated).IsRequired();
        });
    }


    private static void ConfigureStockAlert(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockAlert>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Timestamp);
            e.HasIndex(x => x.ProductId);

            e.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50);

            e.Property(x => x.ProductId).IsRequired();

            e.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500);

            e.Property(x => x.Timestamp).IsRequired();
        });
    }


    private static void ConfigureSalesMetrics(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesMetrics>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.TotalSales).IsRequired();
            e.Property(x => x.ConfirmedSales).IsRequired();
            e.Property(x => x.CanceledSales).IsRequired();

            e.Property(x => x.LastOrderAt).IsRequired(false);
            e.Property(x => x.LastUpdated).IsRequired();
        });
    }
}