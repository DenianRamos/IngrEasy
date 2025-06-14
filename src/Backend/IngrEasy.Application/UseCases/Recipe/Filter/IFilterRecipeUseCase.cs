using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    Task<ResponseRecipesJson> Execute(RequestsFilterRecipeJson request);
}