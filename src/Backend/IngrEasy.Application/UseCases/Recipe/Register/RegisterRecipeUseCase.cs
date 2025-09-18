using AutoMapper;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using IngrEasy.Domain;
using IngrEasy.Domain.Entities;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _recipeWriteOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterRecipeUseCase(IRecipeWriteOnlyRepository recipeWriteOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _recipeWriteOnlyRepository = recipeWriteOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRecipeJson request)
    {
        Validate(request);
        var loggedUser = _loggedUser.User();
        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instruction = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instruction.Count; index++)
            instruction.ElementAt(index).Step = index + 1;

        recipe.Instructions = _mapper.Map<IList<Instruction>>(instruction);

        await _recipeWriteOnlyRepository.Add(recipe);
        
        await _unitOfWork.Commit();
        
        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }


    public static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}