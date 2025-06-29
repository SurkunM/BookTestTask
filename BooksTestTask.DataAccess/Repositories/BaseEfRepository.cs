using BooksTestTask.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess.Repositories;

public class BaseEfRepository<T> : IRepository<T> where T : class
{
    protected BooksDbContext DbContext;

    protected DbSet<T> DbSet;

    public BaseEfRepository(BooksDbContext booksDbContext)
    {
        DbContext = booksDbContext;
        DbSet = DbContext.Set<T>();
    }

    public Task CreateAsync(T entity)
    {
        return DbSet.AddAsync(entity).AsTask();
    }

    public void Delete(T entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        DbSet.Attach(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
    }
}
