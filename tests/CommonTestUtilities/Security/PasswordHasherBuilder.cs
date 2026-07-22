using Cookbook.Domain.Security.PasswordHashing;
using Moq;

namespace CommonTestUtilities.Security;

public sealed class PasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock;

    public PasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(repository => repository.HashPassword(It.IsAny<string>())).Returns("hashed-password");
    }

    public void VerifyPassword(string password)
    {
        _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _mock.Object;
}
