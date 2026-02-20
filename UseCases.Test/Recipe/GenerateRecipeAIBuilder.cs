using IngrEasy.Domain.Dtos;
using IngrEasy.Domain.Services.OpenAI;
using Moq;

namespace UseCases.Test.Recipe;

public class GenerateRecipeAIBuilder
{
    public static IGenerateRecipeAI Build(GeneratedRecipeDto dto)
    {
        var mock = new Mock<IGenerateRecipeAI>();
        mock.Setup(services => services.Generate(It.IsAny<IList<string>>()))
            .ReturnsAsync(dto);
        
        return mock.Object;
    }
}