namespace APICatalogo.Repositories.Interfaces;

public interface IUnitOfWork
{
    Task Commit();
}
