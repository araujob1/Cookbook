using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Api.Controllers;

[Route("[controller]")]
[ApiController]
public abstract class CookbookBaseController : ControllerBase
{
}
