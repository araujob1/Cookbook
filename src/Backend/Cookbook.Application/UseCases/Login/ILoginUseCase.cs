using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.Login;

public interface ILoginUseCase
{
    Task<ResponseRegisterUserJson> ExecuteAsync(RequestLoginJson request);
}
