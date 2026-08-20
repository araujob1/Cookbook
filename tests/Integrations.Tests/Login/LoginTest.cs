using CommonTestUtilities.ClassData;
using CommonTestUtilities.Requests;
using Cookbook.Domain.Extensions;
using Cookbook.Exception.Resources;
using Integrations.Tests.Infrastructure;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Integrations.Tests.Login;

public sealed class LoginTest(CookbookWebApplicationFactory factory) : CookbookClassFixture(factory)
{
    private const string REQUEST_URI = "auth/";

    [Fact]
    public async Task Success()
    {
        var request = RequestLoginJsonBuilder.Build() with
        {
            Email = Seed.Users.User1.Entity.Email.Value,
            Password = Seed.Users.User1.Password
        };

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestCancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestCancellationToken);

        responseData
            .RootElement
            .GetProperty("name")
            .GetString()
            .ShouldBe(Seed.Users.User1.Entity.Name.Value);

        responseData
            .RootElement
            .GetProperty("tokens")
            .GetProperty("accessToken")
            .GetString()
            .ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData<CultureClassData>]
    public async Task Error_User_Dont_Exist(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestCancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestCancellationToken);

        var errorMessages = responseData
            .RootElement
            .GetProperty("errorMessages")
            .EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException
            .ResourceManager
            .GetString("LOGIN_INVALID", new CultureInfo(culture));

        errorMessages.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error =>
                error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}
