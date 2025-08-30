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
using BooksTestTask.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace BooksTestTask;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BooksDbContext>(options =>
        {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("BooksTestTaskConnection"))
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);

        var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["TestCookies"];

                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        builder.Services.AddDefaultIdentity<UserEntity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<BooksDbContext>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("CreateBook", policy =>
                policy.RequireRole("Admin"))
            .AddPolicy("Authenticated", policy =>
                policy.RequireAuthenticatedUser())
            .AddPolicy("AdminOrUser", policy =>
                policy.RequireRole("Admin", "User"))
            .AddPolicy("UpdateBook", policy =>
                policy.RequireRole("Admin"))
            .AddPolicy("DeleteBook", policy =>
                policy.RequireRole("Admin"));

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<BooksDbContext>());
        builder.Services.AddTransient<JwtProvider>();

        builder.Services.AddTransient<IBooksRepository, BooksRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();

        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddTransient<CreateBookHandler>();
        builder.Services.AddTransient<DeleteBookHandler>();
        builder.Services.AddTransient<GetBooksHandler>();
        builder.Services.AddTransient<UpdateBookHandler>();
        builder.Services.AddTransient<AuthenticationUserHandler>();

        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        builder.Services.AddTransient<JwtProvider>();
        builder.Services.AddScoped<DbInitializer>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                dbInitializer.Initialize();
                await dbInitializer.SetRoles(scope.ServiceProvider);
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

        app.UseRouting();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
