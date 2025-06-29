namespace BooksTestTask.Contracts.IUnitOfWork;

public interface IUnitOfWorkTransaction
{
    void BeginTransaction();

    void RollbackTransaction();
}
