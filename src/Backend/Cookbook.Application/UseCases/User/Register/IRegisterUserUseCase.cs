using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> ExecuteAsync(RequestRegisterUserJson request);
}
