using Cookbook.Communication.Responses;
using Cookbook.Exception.Exceptions;
using Cookbook.Exception.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cookbook.Api.Filters;

public sealed class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CookbookException cookbookException)
            HandleProjectException(cookbookException, context);
        else
            ThrowUnknowException(context);
    }

    private static void HandleProjectException(CookbookException cookbookException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)cookbookException.StatusCode;
        context.Result = new ObjectResult(new ResponseErrorJson(cookbookException.ErrorMessages));
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }
}
