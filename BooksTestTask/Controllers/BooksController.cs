using BooksTestTask.BusinessLogic.Handlers.Book;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Model.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksTestTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BooksController : ControllerBase
{
    private readonly CreateBookHandler _createBookHandler;

    private readonly GetBooksHandler _getBooksHandler;

    private readonly UpdateBookHandler _updateBookHandler;

    private readonly DeleteBookHandler _deleteBookHandler;

    private readonly ILogger<BooksController> _logger;

    public BooksController(ILogger<BooksController> logger,
        CreateBookHandler createBookHandler, GetBooksHandler getBooksHandler,
        UpdateBookHandler updateBookHandler, DeleteBookHandler deleteBookHandler)
    {
        _createBookHandler = createBookHandler ?? throw new ArgumentNullException(nameof(createBookHandler));
        _getBooksHandler = getBooksHandler ?? throw new ArgumentNullException(nameof(getBooksHandler));
        _updateBookHandler = updateBookHandler ?? throw new ArgumentNullException(nameof(updateBookHandler));
        _deleteBookHandler = deleteBookHandler ?? throw new ArgumentNullException(nameof(deleteBookHandler));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetBooks()
    {
        var result = await _getBooksHandler.HandleAsync();

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetBook(int id)
    {
        var result = await _getBooksHandler.HandleAsync(id);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBook(BookDto booksDto)
    {
        await _createBookHandler.HandleAsync(booksDto);

        return NoContent();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBook(BookDto booksDto)
    {
        await _updateBookHandler.HandleAsync(booksDto);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook([FromBody] int id)
    {
        await _deleteBookHandler.HandleAsync(id);

        return NoContent();
    }
}
