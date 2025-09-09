using AutoMapper;
using IngrEasy.Communication.Enums;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.Dtos;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase : IFilterRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeReadOnlyRepository _repository;

    public FilterRecipeUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeReadOnlyRepository repository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseRecipesJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);
        
        var loggedUser = _loggedUser.User();


        var filters = new FilterRecipesDto
        {
        RecipeTitle_Ingredient = request.RecipeTitle_Ingredient,
        CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enum.CookingTime)c).ToList(),
        DishTypes = request.DishType.Distinct().Select(d => (Domain.Enum.DishType)d).ToList(),
        Difficulties = request.Difficulty.Distinct().Select(d => (Domain.Enum.Difficulty)d).ToList()
        };
        
        var recipes = await _repository.Filter(await loggedUser, filters);
        
        return new ResponseRecipesJson
        {
         Recipes = _mapper.Map<IList<ResponseShortRecipeJson>>(recipes),
        };
    }



    private static void Validate(RequestFilterRecipeJson request)
    {
        var validator = new FilterRecipeValidator();
        var result = validator.Validate(request);

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).Distinct().ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
            
    }
}