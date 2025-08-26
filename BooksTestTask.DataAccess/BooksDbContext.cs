using BooksTestTask.Contracts.Configuration;
using BooksTestTask.Model;
using BooksTestTask.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BooksTestTask.DataAccess;

public class BooksDbContext : DbContext
{
    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    private readonly AuthorizationOptions _authorizationOptions;

    public BooksDbContext(DbContextOptions<BooksDbContext> options, IOptions<AuthorizationOptions> authorizationOptions) : base(options)
    {
        _authorizationOptions = authorizationOptions.Value ?? throw new ArgumentNullException(nameof(authorizationOptions));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(b =>
        {
            b.Property(b => b.Title).HasMaxLength(50);
            b.Property(b => b.Author).HasMaxLength(50);
            b.Property(b => b.Year).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(b =>
        {
            b.Property(b => b.UserName).HasMaxLength(50);
            b.Property(b => b.Email).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(b =>
        {
            b.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            b.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        });

        modelBuilder.Entity<RolePermission>(b =>
        {
            b.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            b.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        });

        //modelBuilder.Entity<Role>(b =>
        //{
        //    var roles = Enum
        //        .GetValues<RolesEnum>()
        //        .Select(r => new Role
        //        {
        //            Id = (int)r,
        //            Name = r.ToString()
        //        });

        //    b.HasData(roles);
        //});

        //modelBuilder.Entity<Permission>(b =>
        //{
        //    var permissions = Enum
        //        .GetValues<PermissionEnum>()
        //        .Select(p => new Permission
        //        {
        //            Id = (int)p,
        //            Name = p.ToString()
        //        });

        //    b.HasData(permissions);
        //});

        //modelBuilder.Entity<RolePermission>(b =>
        //{
        //    b.HasData(GetRolePermissions());
        //});
    }

    //private RolePermission[] GetRolePermissions()
    //{
    //    return _authorizationOptions.RolePermissions
    //        .SelectMany(rp => rp.Permissions
    //            .Select(p => new RolePermission
    //            {
    //                RoleId = (int)Enum.Parse<RolesEnum>(rp.Role),
    //                PermissionId = (int)Enum.Parse<PermissionEnum>(p)
    //            }))
    //            .ToArray();
    //}
}
