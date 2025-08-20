using BooksTestTask.BusinessLogic.Handlers.Book;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.Model;
using Moq;

namespace BooksTestTask.Tests.HandlersUnitTests;

public class GetBooksHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;

    private readonly GetBooksHandler _getBookHandler;

    public GetBooksHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _getBookHandler = new GetBooksHandler(_uowMock.Object);
    }

    [Fact]
    public async Task HandleAsync_LoadsBooksFromRepository_Successfully()
    {
        var mockBooksData = new List<BookDto>
        {
            new BookDto { Title = "Книга 1", Author = "Автор 1", Year = "2001" },
            new BookDto { Title = "Книга 2", Author = "Автор 2", Year = "2002" }
        };

        var mockBooksRepository = new Mock<IBooksRepository>();
        mockBooksRepository.Setup(repo => repo.GetBooksAsync()).ReturnsAsync(mockBooksData);

        _uowMock.Setup(uow => uow.GetRepository<IBooksRepository>()).Returns(mockBooksRepository.Object);

        var result = await _getBookHandler.HandleAsync();

        Assert.NotNull(result);
        Assert.True(mockBooksData.SequenceEqual(result));
    }

    [Fact]
    public async Task GetBook_ShouldSuccessfullyProcessAllStepsAndSaveChanges()
    {
        int id = 1;

        var mockBook = new Book
        {
            Id = id,
            Title = "Книга 1",
            Author = "Автор 1",
            Year = "2001"
        };

        var mockBooksRepository = new Mock<IBooksRepository>();
        mockBooksRepository.Setup(r => r.FindBookByIdAsync(id)).ReturnsAsync(mockBook);

        _uowMock.Setup(uow => uow.GetRepository<IBooksRepository>()).Returns(mockBooksRepository.Object);

        var result = await _getBookHandler.HandleAsync(id);

        Assert.NotNull(result);
        Assert.True(result.Id == id);
    }

    [Fact]
    public async Task GetBook_ShouldThrowKeyNotFoundException()
    {
        int nonExistentBookId = 999;

        var mockBooksRepository = new Mock<IBooksRepository>();
        mockBooksRepository.Setup(r => r.FindBookByIdAsync(nonExistentBookId)).ThrowsAsync(new KeyNotFoundException());

        _uowMock.Setup(uow => uow.GetRepository<IBooksRepository>()).Returns(mockBooksRepository.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _getBookHandler.HandleAsync(nonExistentBookId));
    }
}
