using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.Model;

public class UserEntity : IdentityUser<Guid>
{
    public required string Login { get; set; }
}
