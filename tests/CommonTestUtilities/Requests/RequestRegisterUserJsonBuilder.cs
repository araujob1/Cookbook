using Bogus;
using Cookbook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        var faker = new Faker();
        var name = faker.Person.FirstName;

        return new RequestRegisterUserJson(
            name,
            faker.Internet.Email(name),
            faker.Internet.Password());
    }
}
