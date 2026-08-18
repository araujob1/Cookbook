using Bogus;
using Cookbook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        var faker = new Faker();
        var name = faker.Person.FirstName;

        return new RequestLoginJson(
            faker.Internet.Email(name),
            faker.Internet.Password());
    }
}
