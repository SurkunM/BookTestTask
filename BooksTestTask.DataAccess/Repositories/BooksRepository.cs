using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksTestTask.DataAccess.Repositories;

public class BooksRepository : BaseEfRepository<Book>, IBooksRepository
{
    public BooksRepository(BooksDbContext booksDbContext, ILogger<BooksRepository> logger) : base(booksDbContext) { }

    public Task<Book?> FindBookByIdAsync(int id)
    {
        return DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<BookDto>> GetBooksAsync()
    {
        return DbSet
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year
            })
            .ToListAsync();
    }

    public Task<BookDto?> GetBookAsync(int id)
    {
        return DbSet
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year
            })
            .FirstOrDefaultAsync();
    }
}
