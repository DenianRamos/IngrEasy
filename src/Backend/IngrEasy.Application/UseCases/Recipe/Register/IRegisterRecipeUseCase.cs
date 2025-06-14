using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request);
}