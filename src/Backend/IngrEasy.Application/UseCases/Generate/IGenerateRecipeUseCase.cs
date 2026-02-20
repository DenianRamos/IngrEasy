using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.Generate;

public interface IGenerateRecipeUseCase
{
    public Task<ResponseGeneratedRecipeJson> Execute(RequestGeneratedRecipeJson request);
}