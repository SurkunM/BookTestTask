using BooksTestTask.BusinessLogic.Authentication;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Dto.Responses;
using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.Extensions;
using BooksTestTask.Contracts.IUnitOfWork;
using BooksTestTask.Model.Identity;
using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.BusinessLogic.Handlers.Users;

public class AuthenticationUserHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly JwtProvider _jwtProvider;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    public AuthenticationUserHandler(IUnitOfWork unitOfWork, JwtProvider jwtProvider,
        UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<UserRegisterResponse> Register(RegisterUserRequest request)
    {
        var user = request.ToUserModel();

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.ToString());
        }

        await _userManager.AddToRoleAsync(user, "Admin");

        var token = await _jwtProvider.GenerateTokenAsync(user);

        return new UserRegisterResponse()
        {
            UserName = user.UserName,
            Token = token
        };
    }

    public async Task<UserRegisterResponse> Login(LoginUserRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new NotFoundException($"Не найден юзер с Email:{request.Email}");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new AuthenticationFailedException("Не удалось авторизоваться");
        }

        var token = await _jwtProvider.GenerateTokenAsync(user);

        return new UserRegisterResponse()
        {
            UserName = user.UserName,
            Token = token
        };
    }
}
