using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Extensions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers;

public class CreateBookHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task HandleAsync(BookDto booksDto)
    {
        var booksRepository = _unitOfWork.GetRepository<IBooksRepository>();

        await booksRepository.CreateAsync(booksDto.ToModel());

        await _unitOfWork.SaveAsync();
    }
}
