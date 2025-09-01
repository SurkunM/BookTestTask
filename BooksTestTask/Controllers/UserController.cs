using BooksTestTask.BusinessLogic.Handlers.Users;
using BooksTestTask.Contracts.Dto;
using Microsoft.AspNetCore.Identity;
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
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        await _createUserHandler.Register(request);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var token = await _createUserHandler.Login(request);

        return Ok(new
        {
            Token = token,
            ExpiresIn = 7200
        });
    }

    [HttpPost]
    public IActionResult SetRole(string email, string role)
    {
        _createUserHandler.SetRolesToUser(email, role);

        return Ok();
    }

}
