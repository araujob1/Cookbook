using System.Text.RegularExpressions;

namespace Cookbook.Domain.ValueObjects;

public readonly partial record struct UserName
{
    private readonly string? _value;

    public UserName(string? value)
    {
        _value = RemoveWhiteSpacesRegex().Replace((value ?? string.Empty).Trim(), " ");
    }

    public string Value => _value ?? string.Empty;

    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveWhiteSpacesRegex();
}
