using Cookbook.Domain.ValueObjects;

namespace Cookbook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmailAsync(Email email);
    Task<bool> ExistActiveUserWithIdAsync(Guid id);
    Task<Entities.User?> GetByEmailAsync(Email email);
}
