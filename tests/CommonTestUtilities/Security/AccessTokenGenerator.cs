using CommonTestUtilities.Generators;
using Cookbook.Domain.Entities;
using Cookbook.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Security;

public sealed class AccessTokenGenerator
{
    public static IAccessTokenGenerator Build(string? accessToken = null)
    {
        accessToken ??= TextGenerator.Words(20);

        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(x => x.Generate(It.IsAny<User>())).Returns(accessToken);

        return mock.Object;
    }
}
