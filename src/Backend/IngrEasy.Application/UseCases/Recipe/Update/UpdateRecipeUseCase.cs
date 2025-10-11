using AutoMapper;
using IngrEasy.Communication.Requests;
using IngrEasy.Domain;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    
    private readonly ILoggedUser _loggedUser;
    private readonly IRecipeUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRecipeUseCase(ILoggedUser loggedUser, IRecipeUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Execute(int recipeId, RequestRecipeJson request)
    {
        Validate(request);

        var loggedUser =  await _loggedUser.User();
        
        var recipe = await _updateOnlyRepository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceErrorMessage.RECIPE_NOT_FOUND);
        
        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes?.Clear();

        _mapper.Map(request, recipe);
        
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();

        for (var index = 0; index < instructions.Count; index++)
        {
            instructions.ElementAt(index).Step = index + 1;
        }
        
        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);
        
        _updateOnlyRepository.Update(recipe);
        await _unitOfWork.Commit();
        
    }


    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if(result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}