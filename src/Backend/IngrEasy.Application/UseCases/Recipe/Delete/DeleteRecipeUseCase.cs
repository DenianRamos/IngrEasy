using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase : IDeleteRecipeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecipeReadOnlyRepository _readOnlyRepository;
    private readonly IRecipeWriteOnlyRepository _writeOnlyRepository;

    public DeleteRecipeUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IRecipeReadOnlyRepository readOnlyRepository, IRecipeWriteOnlyRepository writeOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
    }

    public async Task Execute(int recipeId)
    {
        var logged = await _loggedUser.User();

      var recipe =  await _readOnlyRepository.GetById(logged, recipeId);
      
      if (recipe is null) 
          throw new NotFoundException(ResourceErrorMessage.RECIPE_NOT_FOUND);
      
        await _writeOnlyRepository.Delete(recipeId);
        await _unitOfWork.Commit();
      
    }
}