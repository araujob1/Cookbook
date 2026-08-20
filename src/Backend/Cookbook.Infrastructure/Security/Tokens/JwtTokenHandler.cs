using Cookbook.Domain.Entities;
using Cookbook.Domain.Security.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Cookbook.Infrastructure.Security.Tokens;

internal sealed class JwtTokenHandler(
    uint ExpirationTimeMinutes,
    string SigningKey) : IAccessTokenGenerator
{
    public string Generate(User user)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(ExpirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var handler = new JsonWebTokenHandler();



        return handler.CreateToken(tokenDescriptor);
    }

    private SymmetricSecurityKey SymmetricSecurityKey()
    {
        var keyBytes = Encoding.UTF8.GetBytes(SigningKey);

        return new SymmetricSecurityKey(keyBytes);
    }
}
