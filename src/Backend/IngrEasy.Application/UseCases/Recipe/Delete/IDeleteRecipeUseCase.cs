namespace IngrEasy.Application.UseCases.Recipe.Delete;

public interface IDeleteRecipeUseCase
{
    public Task Execute (int recipeId);
}