using System.ComponentModel.DataAnnotations;

namespace BooksTestTask.Contracts.Dto;

public class RegisterUserRequest
{
    [Required]
    public required string UserName { get; set; }

    [Required]

    public required string Password { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Role { get; set; }
}
