using System.ComponentModel.DataAnnotations;

namespace BooksTestTask.Contracts.Dto;

public class LoginUserRequest
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
