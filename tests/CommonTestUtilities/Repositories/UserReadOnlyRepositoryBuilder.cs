using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.ValueObjects;
using Moq;

namespace CommonTestUtilities.Repositories;

public sealed class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public UserReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmailAsync(Email email)
    {
        _mock.Setup(repository => repository.ExistActiveUserWithEmailAsync(email)).ReturnsAsync(true);
    }

    public void GetByEmailAsync(User user)
    {
        _mock.Setup(repository => repository.GetByEmailAsync(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}
