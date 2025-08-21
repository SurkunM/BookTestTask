using BooksTestTask.Contracts.IRepositories;
using BooksTestTask.Model;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess.Repositories;

public class UserRepository : BaseEfRepository<User>, IUserRepository
{
    public UserRepository(BooksDbContext booksDbContext) : base(booksDbContext)
    {
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
