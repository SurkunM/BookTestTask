using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers;


public class DeleteBookHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> HandleAsync(int id)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var book = await booksRepository.FindBookByIdAsync(id);

            if (book is null)
            {
                return false;
            }

            booksRepository.Delete(book);

            await _unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception)
        {
            _unitOfWork.RollbackTransaction();

            throw;
        }
    }
}
