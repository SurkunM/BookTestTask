using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Extensions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers;

public class UpdateBookHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> HandleAsync(BookDto booksDto)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        try
        {
            _unitOfWork.BeginTransaction();

            var existingBook = await booksRepository.FindBookByIdAsync(booksDto.Id);

            if (existingBook is null)
            {
                _unitOfWork.RollbackTransaction();

                return false;
            }

            booksRepository.Update(booksDto.ToModel());

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
