using BooksTestTask.BusinessLogic.Handlers;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.Model;
using Moq;

namespace BooksTestTask.Tests.HandlersUnitTests;

public class CreateBookHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly CreateBookHandler _createBookHandler;

    public CreateBookHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _createBookHandler = new CreateBookHandler(_uowMock.Object);
    }

    [Fact]
    public async Task ShouldSuccessfullyProcessAllStepsAndSaveChanges()
    {
        var mockBooksRepository = new Mock<IBooksRepository>();
        _uowMock.Setup(uow => uow.GetRepository<IBooksRepository>()).Returns(mockBooksRepository.Object);

        var bookDto = new BookDto
        {
            Id = 1,
            Title = "Книга 1",
            Author = "Автор 1",
            Year = "2001"
        };

        await _createBookHandler.HandleAsync(bookDto);

        mockBooksRepository.Verify(r => r.CreateAsync(It.IsAny<Book>()), Times.Once);
        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}