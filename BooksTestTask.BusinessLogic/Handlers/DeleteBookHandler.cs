using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksTestTask.BusinessLogic.Handlers;


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

        var book = await booksRepository.GetBookByIdAsync(id);

        booksRepository.Delete(book);

        await _unitOfWork.SaveAsync();
    }
}
