using BooksTestTask.BusinessLogic.Handlers.Users;
using BooksTestTask.Contracts.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksTestTask.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly AuthenticationUserHandler _createUserHandler;

    public UserController(AuthenticationUserHandler createUserHandler)
    {
        _createUserHandler = createUserHandler ?? throw new ArgumentNullException(nameof(createUserHandler));
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var result = await _createUserHandler.Register(request);

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _createUserHandler.Login(request);

        return Ok(result);
    }
}
