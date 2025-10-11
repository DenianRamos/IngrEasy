using IngrEasy.Communication.Requests;

namespace IngrEasy.Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    Task Execute(int recipeId, RequestRecipeJson request);
}