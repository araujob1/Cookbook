using CommonTestUtilities.ClassData;
using CommonTestUtilities.Requests;
using Cookbook.Domain.Extensions;
using Cookbook.Exception.Resources;
using Integrations.Tests.Infrastructure;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Integrations.Tests.User.Register;

public sealed class RegisterUserTest(CookbookWebApplicationFactory factory) : CookbookClassFixture(factory)
{
    private const string REQUEST_URI = "users/";

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestCancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestCancellationToken);

        responseData
            .RootElement
            .GetProperty("name")
            .GetString()
            .ShouldNotBeNullOrWhiteSpace();

        responseData
            .RootElement
            .GetProperty("tokens")
            .GetProperty("accessToken")
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

        var response = await Post(REQUEST_URI, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync(TestCancellationToken);

        var responseData = await JsonDocument.ParseAsync(responseBody, cancellationToken: TestCancellationToken);

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
