using BooksTestTask.DataAccess;
using BooksTestTask.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask;

public class DbInitializer
{
    private readonly BooksDbContext _dbContext;

    public DbInitializer(BooksDbContext context)
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Initialize()
    {
        _dbContext.Database.Migrate();
    }

    public async Task SetRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (roleManager.Roles.Any())
        {
            return;
        }

        string[] roleNames = ["Admin", "User"];

        foreach (var roleName in roleNames)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var adminUser = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = "admin",
            Email = "admin@example.com",
            Login = "Administrator",
            PasswordHash = "123"
        };

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = "user1",
            Email = "user@example.com",
            Login = "user123",
            PasswordHash = "123"
        };

        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

        await userManager.CreateAsync(adminUser, "admin");
        await userManager.CreateAsync(user, "user");

        await userManager.AddToRoleAsync(adminUser, "Admin");
        await userManager.AddToRoleAsync(user, "User");
    }
}
