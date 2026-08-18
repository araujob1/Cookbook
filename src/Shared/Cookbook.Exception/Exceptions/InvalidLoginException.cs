using Cookbook.Exception.Resources;
using System.Net;

namespace Cookbook.Exception.Exceptions;

public sealed class InvalidLoginException : CookbookException
{
    public override IList<string> ErrorMessages => [ResourceMessagesException.LOGIN_INVALID];

    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
