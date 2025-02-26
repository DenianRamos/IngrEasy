using Bogus;
using IngrEasy.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLenght = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(x => x.Email, f => f.Internet.Email())

            .RuleFor(x => x.Password, f => f.Internet.Password(passwordLenght));


    }
}