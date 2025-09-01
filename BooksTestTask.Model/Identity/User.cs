using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.Model.Identity;

public class User : IdentityUser<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
