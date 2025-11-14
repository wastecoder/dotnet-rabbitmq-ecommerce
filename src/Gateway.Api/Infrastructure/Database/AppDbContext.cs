using Gateway.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserAccount> Users => Set<UserAccount>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
}