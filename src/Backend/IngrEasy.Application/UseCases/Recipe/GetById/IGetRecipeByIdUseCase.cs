using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    public Task<ResponseRecipeJson> Execute(int recipeId);
}