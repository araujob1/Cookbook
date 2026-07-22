using CommonTestUtilities.ClassData;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using Cookbook.Application.UseCases.User.Register;
using Cookbook.Domain.ValueObjects;
using Cookbook.Exception.Exceptions;
using Cookbook.Exception.Resources;
using Shouldly;

namespace UseCases.Tests.User.Register;

public sealed class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.ShouldNotBeNull();
        result.Name.ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData<InvalidStringClassData>]
    public async Task Error_Name_Required(string? invalidName)
    {
        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Name = invalidName!
        };

        var useCase = CreateUseCase();

        var exception = await useCase.ExecuteAsync(request)
            .ShouldThrowAsync<ErrorOnValidationException>();

        exception.ErrorMessages.ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.NAME_REQUIRED);
        });
    }

    [Fact]
    public async Task Error_Email_Already_In_Use()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var exception = await useCase.ExecuteAsync(request)
            .ShouldThrowAsync<ErrorOnValidationException>();

        exception.ErrorMessages.ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.EMAIL_ALREADY_IN_USE);
        });
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new PasswordHasherBuilder().Build();

        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (email is not null)
            readOnlyRepository.ExistActiveUserWithEmailAsync(new Email(email));

        return new RegisterUserUseCase(unitOfWork, readOnlyRepository.Build(), writeOnlyRepository, passwordHasher);
    }
}
