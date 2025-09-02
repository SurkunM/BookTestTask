using BooksTestTask.DataAccess;
using BooksTestTask.Model.Enums;
using BooksTestTask.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask;

public static class DbInitializer
{
    public static async Task BooksDbInitialize(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        try
        {
            var service = scope.ServiceProvider;

            var booksDbContext = service.GetRequiredService<BooksDbContext>();
            await booksDbContext.Database.MigrateAsync();

            var identityDbContext = service.GetRequiredService<IdentityDbContext>();
            await identityDbContext.Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            await SeedData(service);
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "При создании базы данных произошла ошибка.");

            throw;
        }
    }

    private static async Task SeedData(IServiceProvider service)
    {
        var roleManager = service.GetRequiredService<RoleManager<Role>>();

        if (roleManager.Roles.Any())
        {
            return;
        }

        var roleNames = Enum.GetNames<RolesEnum>();

        foreach (var roleName in roleNames)
        {
            await roleManager.CreateAsync(new Role { Name = roleName });
        }

        var userManager = service.GetRequiredService<UserManager<User>>();

        if (userManager.Users.Any())
        {
            return;
        }

        var adminUser = new User
        {
            UserName = "admin",
            Email = "admin@example.com"
        };

        var user = new User
        {
            UserName = "user1",
            Email = "user@example.com"
        };

        await userManager.CreateAsync(adminUser, "admin");
        await userManager.CreateAsync(user, "user");

        await userManager.AddToRoleAsync(adminUser, "Admin");
        await userManager.AddToRoleAsync(user, "User");
    }
}
