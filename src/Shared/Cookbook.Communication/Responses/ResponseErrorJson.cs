namespace Cookbook.Communication.Responses;

public sealed record ResponseErrorJson(IList<string> ErrorMessages)
{
    public ResponseErrorJson(string errorMessage) : this([errorMessage]) { }
};
