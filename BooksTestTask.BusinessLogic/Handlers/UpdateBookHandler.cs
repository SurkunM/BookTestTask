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

    public async Task HandleAsync(BookDto booksDto)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        booksRepository.Update(booksDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
