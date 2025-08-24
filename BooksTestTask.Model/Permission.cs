namespace BooksTestTask.Model;

public class Permission
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}
