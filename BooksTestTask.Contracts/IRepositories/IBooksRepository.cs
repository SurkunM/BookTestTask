using BooksTestTask.Contracts.Dto;
using BooksTestTask.Model;

namespace BooksTestTask.Contracts.IRepositories;

public interface IBooksRepository : IRepository<Book>
{
    Task<List<BookDto>> GetBooksAsync();

    Task<Book?> FindBookByIdAsync(int id);

    Task<BookDto?> GetBookAsync(int id);
}
