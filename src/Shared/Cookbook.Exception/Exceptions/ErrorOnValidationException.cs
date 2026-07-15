using System.Net;

namespace Cookbook.Exception.Exceptions;

public sealed class ErrorOnValidationException(IList<string> errorMessages) : CookbookException
{
    public override IList<string> ErrorMessages => errorMessages;

    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
