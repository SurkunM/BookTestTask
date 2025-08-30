using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.Model;

public class UserEntity : IdentityUser
{
    public required string Login { get; set; }
}
