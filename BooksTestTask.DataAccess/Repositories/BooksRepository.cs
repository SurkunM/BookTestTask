using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksTestTask.DataAccess.Repositories;

public class BooksRepository : BaseEfRepository<Book>, IBooksRepository
{
    public BooksRepository(BooksDbContext booksDbContext, ILogger<BooksRepository> logger) : base(booksDbContext) { }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        var book = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

        return book is null ? throw new KeyNotFoundException() : book;
    }

    public Task<List<BookDto>> GetBooksAsync()
    {
        return DbSet
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year,
            })
            .ToListAsync();
    }
}
