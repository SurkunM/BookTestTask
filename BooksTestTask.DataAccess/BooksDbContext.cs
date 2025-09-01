using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess;

public class BooksDbContext : DbContext
{
    public virtual DbSet<Book> Books { get; set; }

    public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(b =>
        {
            b.Property(b => b.Title).HasMaxLength(50);
            b.Property(b => b.Author).HasMaxLength(50);
            b.Property(b => b.Year).HasMaxLength(50);
        });
    }
}
