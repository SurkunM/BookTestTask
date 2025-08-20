using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers.Book;

public class DeleteBookHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task HandleAsync(int id)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var book = await booksRepository.FindBookByIdAsync(id);

            if (book is null)
            {
                _unitOfWork.RollbackTransaction();

                throw new NotFoundException("Книга не найдена");
            }

            booksRepository.Delete(book);

            await _unitOfWork.SaveAsync();
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
