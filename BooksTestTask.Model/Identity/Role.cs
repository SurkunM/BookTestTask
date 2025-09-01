using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.Model.Identity;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
