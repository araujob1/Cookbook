using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.PasswordHashing;
using Cookbook.Domain.ValueObjects;
using Cookbook.Exception.Exceptions;

namespace Cookbook.Application.UseCases.Login;

public sealed class LoginUseCase(
    IUserReadOnlyRepository readOnlyRepository,
    IPasswordHasher passwordHasher) : ILoginUseCase
{
    public async Task<ResponseRegisterUserJson> ExecuteAsync(RequestLoginJson request)
    {
        var user = await readOnlyRepository.GetByEmailAsync(new Email(request.Email))
            ?? throw new InvalidLoginException();

        var isPasswordValid = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (isPasswordValid is false)
            throw new InvalidLoginException();

        return new ResponseRegisterUserJson(user.Name.Value);
    }
}
