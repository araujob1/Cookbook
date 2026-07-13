namespace Cookbook.Domain.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();
}
