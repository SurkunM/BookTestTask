using BooksTestTask.Model;

namespace BooksTestTask.Contracts.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmail(string email);
}
