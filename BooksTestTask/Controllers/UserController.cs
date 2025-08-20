using BooksTestTask.BusinessLogic.Handlers.User;
using BooksTestTask.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BooksTestTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly CreateUserHandler _createUserHandler;

    public UserController(CreateUserHandler createUserHandler)
    {
        _createUserHandler = createUserHandler ?? throw new ArgumentNullException(nameof(createUserHandler));
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        await _createUserHandler.Register(request);

        return Ok();
    }

    [HttpPost]
    public IActionResult Login(LoginUserRequest request)
    {
        var token = _createUserHandler.Login(request);

        return Ok(token.Result);
    }
}
