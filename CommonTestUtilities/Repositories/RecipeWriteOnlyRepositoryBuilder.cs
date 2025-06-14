using IngrEasy.Domain.Entities;
using IngrEasy.Domain.Repositories.Recipe;
using Moq;

namespace CommonTestUtilities.Repositories;

public class RecipeWriteOnlyRepositoryBuilder
{
    public static IRecipeWriteOnlyRepository Build()
    {
        var mock = new Mock<IRecipeWriteOnlyRepository>();
        
        return mock.Object;
    }
}