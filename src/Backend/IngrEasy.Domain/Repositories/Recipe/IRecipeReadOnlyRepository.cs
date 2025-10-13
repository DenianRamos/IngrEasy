using IngrEasy.Domain.Dtos;

namespace IngrEasy.Domain.Repositories.Recipe;

public interface IRecipeReadOnlyRepository
{
    Task <IList<Domain.Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);

    
    Task<Domain.Entities.Recipe?> GetById(Entities.User user, int recipeId);
    
    Task<IList<Entities.Recipe>>GetForDashboard(Entities.User user);
}