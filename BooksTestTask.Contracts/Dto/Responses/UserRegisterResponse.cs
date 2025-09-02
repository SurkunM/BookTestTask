namespace BooksTestTask.Contracts.Dto.Responses;

public class UserRegisterResponse
{
    public Guid Id { get; set; }

    public required string UserName { get; set; }

    public required string Token { get; set; }
}
