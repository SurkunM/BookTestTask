using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers.Book;

public class GetBooksHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBooksHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<List<BookDto>> HandleAsync()
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        return booksRepository.GetBooksAsync();
    }

    public async Task<BookDto> HandleAsync(int id)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        var book = await booksRepository.GetBookAsync(id);

        return book is null ? throw new NotFoundException("Книга не найдена") : book;
    }
}
