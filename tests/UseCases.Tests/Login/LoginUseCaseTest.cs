using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Cookbook.Application.UseCases.Login;
using Cookbook.Domain.Extensions;
using Cookbook.Exception.Exceptions;
using Cookbook.Exception.Resources;
using Shouldly;

namespace UseCases.Tests.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build() with
        {
            Email = user.Email.Value
        };

        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.ExecuteAsync(request);

        result.ShouldNotBeNull();
        result.Name.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Dont_Exist()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var exception = await useCase.ExecuteAsync(request)
            .ShouldThrowAsync<InvalidLoginException>();

        exception.ErrorMessages.ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.LOGIN_INVALID);
        });
    }

    [Fact]
    public async Task Error_Password_Is_Invalid()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build() with
        {
            Email = user.Email.Value
        };

        var useCase = CreateUseCase(user: user);

        var exception = await useCase.ExecuteAsync(request)
            .ShouldThrowAsync<InvalidLoginException>();

        exception.ErrorMessages.ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.LOGIN_INVALID);
        });
    }

    private static LoginUseCase CreateUseCase(
        Cookbook.Domain.Entities.User? user = null,
        string? password = null,
        string? accessToken = null)
    {
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var passwordHasherBuilder = new PasswordHasherBuilder();
        var accessTokenGenerator = AccessTokenGenerator.Build(accessToken);

        if (user is not null)
            readOnlyRepository.GetByEmailAsync(user);

        if (password.IsNotEmpty())
            passwordHasherBuilder.VerifyPassword(password);

        return new LoginUseCase(
            readOnlyRepository.Build(),
            passwordHasherBuilder.Build(),
            accessTokenGenerator);
    }
}
