using BooksTestTask.BusinessLogic.Authentication;
using BooksTestTask.BusinessLogic.Handlers.Book;
using BooksTestTask.BusinessLogic.Handlers.User;
using BooksTestTask.BusinessLogic.Middleware;
using BooksTestTask.Configuration;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.DataAccess;
using BooksTestTask.DataAccess.Repositories;
using BooksTestTask.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BooksDbContext>(options =>
        {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("BooksTestTaskConnection"))
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<BooksDbContext>());

        builder.Services.AddTransient<IBooksRepository, BooksRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();

        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient<CreateBookHandler>();
        builder.Services.AddTransient<DeleteBookHandler>();
        builder.Services.AddTransient<GetBooksHandler>();
        builder.Services.AddTransient<UpdateBookHandler>();
        builder.Services.AddTransient<CreateUserHandler>();

        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        builder.Services.AddTransient<JwtProvider>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
                dbInitializer.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "При создании базы данных произошла ошибка.");

                throw;
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}
