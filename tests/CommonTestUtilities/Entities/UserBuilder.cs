using Bogus;
using CommonTestUtilities.Security;
using Cookbook.Domain.Entities;
using Cookbook.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static (User user, string password) Build()
    {
        var faker = new Faker();
        var userName = new UserName(faker.Person.FirstName);

        var (password, passwordHashed) = GenerateRandomPassword();

        var user = new Faker<User>()
            .RuleFor(user => user.Name, _ => userName)
            .RuleFor(user => user.Email, (faker, user) => new Email(faker.Internet.Email(user.Name.Value)))
            .RuleFor(user => user.PasswordHash, _ => passwordHashed);

        return (user, password);
    }

    private static (string password, string passwordHashed) GenerateRandomPassword()
    {
        var passwordHasherBuilder = new PasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return (password, passwordHasherBuilder.HashPassword(password));
    }
}
