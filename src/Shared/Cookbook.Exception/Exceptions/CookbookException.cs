using System.Net;

namespace Cookbook.Exception.Exceptions;

public abstract class CookbookException : System.Exception
{
    public abstract IList<string> ErrorMessages { get; }
    public abstract HttpStatusCode StatusCode { get; }
}
