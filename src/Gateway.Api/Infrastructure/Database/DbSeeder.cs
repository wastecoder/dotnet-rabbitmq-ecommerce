using Gateway.Api.Domain.Entities;
using Gateway.Api.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Infrastructure.Database;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync())
            return;

        var hasher = new PasswordHasher<UserAccount>();

        var admin = new UserAccount
        {
            Id = Guid.NewGuid(),
            Email = "admin@admin.com",
            FullName = "Administrator",
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };
        admin.PasswordHash = hasher.HashPassword(admin, "123456");

        var user = new UserAccount
        {
            Id = Guid.NewGuid(),
            Email = "user@user.com",
            FullName = "Regular User",
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = hasher.HashPassword(user, "123456");

        db.Users.AddRange(admin, user);
        await db.SaveChangesAsync();
    }
}