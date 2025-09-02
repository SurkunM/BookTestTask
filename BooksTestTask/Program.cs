using BooksTestTask.BusinessLogic.Authentication;
using BooksTestTask.BusinessLogic.Handlers.Book;
using BooksTestTask.BusinessLogic.Handlers.Users;
using BooksTestTask.BusinessLogic.Middleware;
using BooksTestTask.Configuration;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.DataAccess;
using BooksTestTask.DataAccess.Repositories;
using BooksTestTask.DataAccess.UnitOfWork;
using BooksTestTask.Model.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

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

        builder.Services.AddDbContext<IdentityDbContext>(options =>
        {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("BooksTestTaskConnection"))
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);

        builder.Services
            .AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

        var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        });

        builder.Services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

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
        builder.Services.AddTransient<AuthenticationUserHandler>();

        builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
        builder.Services.AddTransient<JwtProvider>();

        var app = builder.Build();

        await app.BooksDbInitialize();

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
