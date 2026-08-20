using Cookbook.Domain.Entities;

namespace Cookbook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
