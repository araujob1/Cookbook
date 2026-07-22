using CommonTestUtilities.ClassData;
using CommonTestUtilities.Requests;
using Cookbook.Domain.Extensions;
using Cookbook.Exception.Resources;
using Integrations.Tests.Infrastructure;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Integrations.Tests.User.Register;

public sealed class RegisterUserTest(CookbookWebApplicationFactory factory) : CookbookClassFixture(factory)
{
    private const string REQUEST_URI = "users/";

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var cancellationToken = TestContext.Current.CancellationToken;

        var response = await Client.PostAsJsonAsync(REQUEST_URI, request, cancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync(cancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: cancellationToken);

        responseData
            .RootElement
            .GetProperty("name")
            .GetString()
            .ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData<CultureClassData>]
    public async Task Error_Name_Required(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Name = string.Empty
        };
        var cancellationToken = TestContext.Current.CancellationToken;

        Client.DefaultRequestHeaders.AcceptLanguage.Clear();
        Client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await Client.PostAsJsonAsync(REQUEST_URI, request, cancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync(cancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: cancellationToken);

        var errorMessages = responseData
            .RootElement
            .GetProperty("errorMessages")
            .EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException
            .ResourceManager
            .GetString("NAME_REQUIRED", new CultureInfo(culture));

        errorMessages.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
