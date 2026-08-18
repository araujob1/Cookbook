using Cookbook.Domain.ValueObjects;

namespace Cookbook.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmailAsync(Email email);
    Task<Entities.User?> GetByEmailAsync(Email email);
}
