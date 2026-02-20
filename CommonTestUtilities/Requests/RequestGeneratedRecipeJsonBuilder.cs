using Bogus;
using IngrEasy.Communication.Response;

namespace CommonTestUtilities.Requests;

public class RequestGeneratedRecipeJsonBuilder
{
    public static RequestGeneratedRecipeJson Build(int count = 5)
    {
        return new Faker<RequestGeneratedRecipeJson>()
            .RuleFor(user => user.Ingredients, faker => faker.Make(count, () => faker.Commerce.ProductName()));
    }
}