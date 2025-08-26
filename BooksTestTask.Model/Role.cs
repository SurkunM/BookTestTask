namespace BooksTestTask.Model;

public class Role
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];

    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}
