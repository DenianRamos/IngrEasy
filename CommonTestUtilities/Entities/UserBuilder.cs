using Bogus;
using CommonTestUtilities.Cryptography;
using IngrEasy.Application.Services.Cryptography;
using IngrEasy.Domain;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        var password = new Faker().Internet.Password();
        var user = new Faker<IngrEasy.Domain.User>()
            .RuleFor(user => user.Id, () => 1)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.Password, () => passwordEncrypter.Encrypt(password))
            .RuleFor(user => user.Name, (f) => f.Person.FirstName);
        return (user, password);
    }
}