using BooksTestTask.BusinessLogic.Handlers;
using BooksTestTask.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BooksTestTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BooksTestTaskController : ControllerBase
{
    private readonly CreateBookHandler _createBookHandler;

    private readonly GetBooksHandler _getBooksHandler;

    private readonly UpdateBookHandler _updateBookHandler;

    private readonly DeleteBookHandler _deleteBookHandler;

    private readonly ILogger<BooksTestTaskController> _logger;

    public BooksTestTaskController(ILogger<BooksTestTaskController> logger,
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
    public async Task<IActionResult> GetBooks()
    {
        var result = await _getBooksHandler.HandleAsync();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetBook(int id)
    {
        var result = await _getBooksHandler.HandleAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookDto booksDto)
    {
        await _createBookHandler.HandleAsync(booksDto);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook(BookDto booksDto)
    {
        var success = await _updateBookHandler.HandleAsync(booksDto);

        if (!success)
        {
            return BadRequest("Книга не найдена");
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBook([FromBody] int id)
    {

        var success = await _deleteBookHandler.HandleAsync(id);

        if (!success)
        {
            return BadRequest("Книга не найдена");
        }

        return NoContent();
    }
}
