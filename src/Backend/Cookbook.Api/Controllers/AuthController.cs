using Cookbook.Application.UseCases.Login;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Api.Controllers;

public sealed class AuthController : CookbookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] RequestLoginJson request,
        [FromServices] ILoginUseCase useCase)
    {
        var response = await useCase.ExecuteAsync(request);

        return Ok(response);
    }
}
