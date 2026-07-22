using CommonTestUtilities.ClassData;
using CommonTestUtilities.Generators;
using CommonTestUtilities.Requests;
using Cookbook.Application.UseCases.User.Register;
using Cookbook.Exception.Resources;
using Shouldly;

namespace Validators.Tests.User.Register;

public sealed class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [ClassData<InvalidStringClassData>]
    public void Error_Name_Required(string? invalidName)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Name = invalidName!
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.NAME_REQUIRED));
        });
    }

    [Fact]
    public void Error_Name_Max_Length()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Name = TextGenerator.Words(101)
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.NAME_MAX_LENGTH));
        });
    }

    [Theory]
    [ClassData<InvalidStringClassData>]
    public void Error_Email_Required(string? invalidEmail)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Email = invalidEmail!
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void Error_Email_Max_Length()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Email = $"{TextGenerator.Words(252)}@b.c"
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.EMAIL_MAX_LENGTH));
        });
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Email = "invalid-email"
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
        });
    }

    [Theory]
    [ClassData<InvalidStringClassData>]
    public void Error_Password_Required(string? invalidPassword)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Password = invalidPassword!
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_REQUIRED));
        });
    }

    [Fact]
    public void Error_Password_Max_Length()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build() with
        {
            Password = TextGenerator.Paragraphs(2001)
        };

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(validationFailures =>
        {
            validationFailures.Count.ShouldBe(1);
            validationFailures.ShouldContain(validationFailure =>
                validationFailure.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_MAX_LENGTH));
        });
    }
}
