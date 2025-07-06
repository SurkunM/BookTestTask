using BooksTestTask.BusinessLogic.Handlers;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.Model;
using Moq;

namespace BooksTestTask.Tests.HandlersUnitTests;

public class DeleteBookHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly DeleteBookHandler _deleteBookHandler;

    public DeleteBookHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _deleteBookHandler = new DeleteBookHandler(_uowMock.Object);
    }

    [Fact]
    public async Task ShouldSuccessfullyProcessAllStepsAndSaveChanges()
    {
        int id = 1;

        var mockBook = new Book
        {
            Id = id,
            Title = "Война и мир",
            Author = "Лев Толстой",
            Year = "1873"
        };

        var mockBooksRepository = new Mock<IBooksRepository>();
        mockBooksRepository.Setup(r => r.FindBookByIdAsync(id)).ReturnsAsync(mockBook);

        _uowMock.Setup(uow => uow.GetRepository<IBooksRepository>()).Returns(mockBooksRepository.Object);

        await _deleteBookHandler.HandleAsync(id);

        mockBooksRepository.Verify(r => r.FindBookByIdAsync(id), Times.Once());
        mockBooksRepository.Verify(r => r.Delete(mockBook), Times.Once);

        _uowMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}
