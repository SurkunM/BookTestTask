namespace BooksTestTask.Contracts.IRepositories;

public interface IRepository
{
}

public interface IRepository<T> : IRepository
{
    Task CreateAsync(T entity);

    void Update(T entity);

    void Delete(T entity);
}