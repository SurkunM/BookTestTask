using BooksTestTask.BusinessLogic.Authentication;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.Extensions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;
using Microsoft.AspNetCore.Identity;
using BooksTestTask.Model.Identity;

namespace BooksTestTask.BusinessLogic.Handlers.Users;

public class AuthenticationUserHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher _passwordHasher;

    private readonly JwtProvider _jwtProvider;

    private readonly UserManager<User> _userManager;

    public AuthenticationUserHandler(IUnitOfWork unitOfWork, IPasswordHasher password, JwtProvider jwtProvider, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = password ?? throw new ArgumentNullException(nameof(password));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task Register(RegisterUserRequest request)
    {
        var hashedPassword = _passwordHasher.Generate(request.Password);

        var user = request.ToUserModel(hashedPassword);

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.ToString());
        }

        await _userManager.AddToRoleAsync(user, "User");

        await _jwtProvider.GenerateTokenAsync(user);
    }

    public async Task<string> Login(LoginUserRequest request)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();

        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            throw new NotFoundException($"Не найден юзер с Email:{request.Email}");
        }

        var result = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!result)
        {
            throw new AuthenticationFailedException("Не удалось авторизоваться");
        }

        return await _jwtProvider.GenerateTokenAsync(user);
    }

    public void SetRolesToUser(string email, string role)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();

        //userRepository.SetRoles(email, role);

        _unitOfWork.SaveAsync();
    }
}
