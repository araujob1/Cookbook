using Cookbook.Domain.ValueObjects;

namespace Cookbook.Domain.Entities;

public sealed class User : EntityBase
{
    public UserName Name { get; set; }
    public Email Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}
