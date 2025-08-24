namespace BooksTestTask.Model;

public class RolePermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public virtual required Role Role { get; set; }

    public int PermissionId { get; set; }

    public virtual required Permission Permission { get; set; }
}
