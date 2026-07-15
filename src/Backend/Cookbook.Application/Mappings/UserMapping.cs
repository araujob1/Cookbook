using Cookbook.Communication.Requests;
using Cookbook.Domain.Entities;
using Cookbook.Domain.ValueObjects;

namespace Cookbook.Application.Mappings;

public static class UserMapping
{
    public static User ToEntity(this RequestRegisterUserJson request, string passwordHash) =>
        new()
        {
            Name = new UserName(request.Name),
            Email = new Email(request.Email),
            PasswordHash = passwordHash
        };
}
