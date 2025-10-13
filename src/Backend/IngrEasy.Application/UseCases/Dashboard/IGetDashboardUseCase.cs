using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.Dashboard;

public interface IGetDashboardUseCase
{
    Task<ResponseRecipesJson> Execute();
}