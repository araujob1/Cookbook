using Cookbook.Application.Mappings;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.PasswordHashing;
using Cookbook.Domain.ValueObjects;
using Cookbook.Exception.Exceptions;
using Cookbook.Exception.Resources;
using FluentValidation.Results;

namespace Cookbook.Application.UseCases.User.Register;

public sealed class RegisterUserUseCase(
    IUnitOfWork unitOfWork,
    IUserReadOnlyRepository readOnlyRepository,
    IUserWriteOnlyRepository writeOnlyRepository,
    IPasswordHasher passwordHasher) : IRegisterUserUseCase
{
    public async Task<ResponseRegisterUserJson> ExecuteAsync(RequestRegisterUserJson request)
    {
        await ValidateRequest(request);

        var passwordHash = passwordHasher.HashPassword(request.Password);
        var user = request.ToEntity(passwordHash);

        await writeOnlyRepository.AddAsync(user);

        await unitOfWork.CommitAsync();

        return new ResponseRegisterUserJson(user.Name.Value);
    }

    private async Task ValidateRequest(RequestRegisterUserJson request)
    {
        var result = new RegisterUserValidator()
            .Validate(request);

        var existActiveUserWithEmail = await readOnlyRepository
            .ExistActiveUserWithEmailAsync(new Email(request.Email));

        if (existActiveUserWithEmail)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_IN_USE));

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
    }
}
