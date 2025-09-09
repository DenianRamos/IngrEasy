using Bogus;
using IngrEasy.Communication.Enums;
using IngrEasy.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestFilterRecipeJsonBuilder
{
    public static RequestFilterRecipeJson Build()
    {
        return new Faker<RequestFilterRecipeJson>()
            .RuleFor(user => user.CookingTimes, faker => faker.Make(1, () => faker.PickRandom<CookingTime>()))
            .RuleFor(user => user.Difficulty, faker => faker.Make(1, () => faker.PickRandom<Difficulty>()))
            .RuleFor(user => user.DishType, faker => faker.Make(1, () => faker.PickRandom<DishType>()))
            .RuleFor(user => user.RecipeTitle_Ingredient, faker => faker.Lorem.Word());
    }
}