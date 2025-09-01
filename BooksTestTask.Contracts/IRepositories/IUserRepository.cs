using BooksTestTask.Model;
using BooksTestTask.Model.Identity;

namespace BooksTestTask.Contracts.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
