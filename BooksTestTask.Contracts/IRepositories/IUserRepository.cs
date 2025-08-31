using BooksTestTask.Model;

namespace BooksTestTask.Contracts.IRepositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
}
