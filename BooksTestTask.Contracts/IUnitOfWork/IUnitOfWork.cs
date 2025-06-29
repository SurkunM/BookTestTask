namespace BooksTestTask.Contracts.IUnitOfWork;

public interface IUnitOfWork : IUnitOfWorkTransaction, IDisposable
{
    Task SaveAsync();

    T GetRepository<T>() where T : class;
}
