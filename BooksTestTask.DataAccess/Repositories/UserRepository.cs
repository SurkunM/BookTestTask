using BooksTestTask.Contracts.Exceptions;
using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess.Repositories;

public class UserRepository : BaseEfRepository<UserEntity>, IUserRepository
{
    public UserRepository(BooksDbContext booksDbContext) : base(booksDbContext)
    {
    }

    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        return DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
