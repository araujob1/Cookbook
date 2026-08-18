using System.Net.Http.Json;

namespace Integrations.Tests.Infrastructure;

public abstract class CookbookClassFixture(CookbookWebApplicationFactory factory) : IClassFixture<CookbookWebApplicationFactory>
{
    protected HttpClient Client { get; } = factory.CreateClient();

    protected DatabaseSeed Seed => factory.Seed;

    protected static CancellationToken TestCancellationToken =>
        TestContext.Current.CancellationToken;

    protected async Task<HttpResponseMessage> Post(
        string requestUri,
        object request,
        string culture = "en")
    {
        ChangeRequestCulture(culture);

        return await Client.PostAsJsonAsync(requestUri, request, TestCancellationToken);
    }

    private void ChangeRequestCulture(string culture)
    {
        Client.DefaultRequestHeaders.AcceptLanguage.Clear();
        Client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }
}
