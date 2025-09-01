using Microsoft.AspNetCore.Identity;

namespace BooksTestTask.Model.Identity;

public class UserRole : IdentityUserRole<Guid>
{
    public required virtual User User { get; set; }

    public required virtual Role Role { get; set; }
}
