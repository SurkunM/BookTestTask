using BooksTestTask.BusinessLogic.Authentication;
using BooksTestTask.Contracts.Dto;
using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.Extensions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Contracts.IUnitOfWork;

namespace BooksTestTask.BusinessLogic.Handlers.User;

public class CreateUserHandler
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IPasswordHasher _passwordHasher;

    private readonly JwtProvider _jwtProvider;

    public CreateUserHandler(IUnitOfWork unitOfWork, IPasswordHasher password, JwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = password ?? throw new ArgumentNullException(nameof(password));
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
    }

    public async Task Register(RegisterUserRequest request)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();

        var hashedPassword = _passwordHasher.Generate(request.Password);

        await userRepository.CreateAsync(request.ToUserModel(hashedPassword));

        await _unitOfWork.SaveAsync();
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

        //Учебный способ:
        if (!result)
        {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}
