namespace Integrations.Tests.Infrastructure;

public abstract class CookbookClassFixture(CookbookWebApplicationFactory factory) : IClassFixture<CookbookWebApplicationFactory>
{
    protected HttpClient Client { get; } = factory.CreateClient();
}
