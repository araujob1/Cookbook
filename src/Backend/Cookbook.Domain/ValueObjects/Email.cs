namespace Cookbook.Domain.ValueObjects;

public readonly record struct Email
{
    private readonly string? _value;

    public Email(string? value)
    {
        _value = (value ?? string.Empty).Trim().ToLowerInvariant();
    }

    public string Value => _value ?? string.Empty;
}
