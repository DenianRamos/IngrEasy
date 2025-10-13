using AutoMapper;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Services.LoggedUser;

namespace IngrEasy.Application.UseCases.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetDashboardUseCase(ILoggedUser loggedUser, IRecipeReadOnlyRepository repository, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseRecipesJson> Execute()
    {
        var loggedUser = _loggedUser.User();
        var recipes = await _repository.GetForDashboard(await loggedUser);

        return new ResponseRecipesJson
        {
            Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };
    }
}