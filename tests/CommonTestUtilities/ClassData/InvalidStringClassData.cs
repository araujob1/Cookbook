using Xunit;

namespace CommonTestUtilities.ClassData;

public sealed class InvalidStringClassData()
    : TheoryData<string?>(string.Empty, " ", null)
{
}
