namespace Cookbook.Communication.Responses;

public sealed record ResponseErrorJson(
    IList<string> ErrorMessages,
    bool AccessTokenExpired = false)
{
    public ResponseErrorJson(string errorMessage) : this([errorMessage]) { }

    public ResponseErrorJson(string errorMessage, bool accessTokenExpired)
        : this([errorMessage], accessTokenExpired) { }
};
