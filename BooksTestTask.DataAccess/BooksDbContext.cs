using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess;

public class BooksDbContext : DbContext
{
    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options) { }

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
    }
}
