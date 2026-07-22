using Bogus;
using System.Text;

namespace CommonTestUtilities.Generators;

public static class TextGenerator
{
    private static readonly Faker Faker = new();

    public static string Words(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        if (length == 0)
            return string.Empty;

        var builder = new StringBuilder();

        while (builder.Length < length)
            builder.Append(string.Concat(Faker.Lorem.Words()));

        return builder.ToString()[..length];
    }

    public static string Paragraphs(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        if (length == 0)
            return string.Empty;

        var builder = new StringBuilder();

        while (builder.Length < length)
            builder.Append(Faker.Lorem.Paragraphs());

        return builder.ToString()[..length];
    }
}
