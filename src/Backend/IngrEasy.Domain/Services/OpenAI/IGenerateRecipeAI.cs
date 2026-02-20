using IngrEasy.Domain.Dtos;

namespace IngrEasy.Domain.Services.OpenAI;

public interface IGenerateRecipeAI
{
    Task<GeneratedRecipeDto> Generate(IList<string> ingredients);
}