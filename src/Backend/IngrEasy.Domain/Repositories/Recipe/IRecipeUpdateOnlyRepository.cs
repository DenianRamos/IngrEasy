namespace IngrEasy.Domain.Repositories.Recipe;

public interface IRecipeUpdateOnlyRepository
{
    Task<Entities.Recipe> GetById(Entities.User user,int recipeId);
    void Update(Entities.Recipe recipe);
}