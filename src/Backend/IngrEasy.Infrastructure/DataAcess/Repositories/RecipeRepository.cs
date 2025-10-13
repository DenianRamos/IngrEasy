using IngrEasy.Domain.Dtos;
using IngrEasy.Domain.Entities;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.Recipe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IngrEasy.Infrastructure.DataAcess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{

    private readonly IngrEasyDbContext _dbContext;

    public RecipeRepository(IngrEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
    public async Task Delete(int recipeId)
    {
        var recipe = await _dbContext.Recipes.FindAsync(recipeId);

        _dbContext.Recipes.Remove(recipe!);
    }

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
    {
        var query = _dbContext.Recipes.AsNoTracking().Include(recipe => recipe.Ingredients).Where(recipe => recipe.Active && recipe.UserId == user.Id);


        if (filters.Difficulties.Any())
        {
            query = query.Where(recipe =>
                recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
        }

        if (filters.CookingTimes.Any())
        {
            query = query.Where(recipe =>
                recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
        }

        if (filters.DishTypes.Any())
        {
            query = query.Where(recipe => recipe.DishTypes!.Any(dishType => filters.DishTypes.Contains(dishType.Type)));
        }

        if (filters.RecipeTitle_Ingredient.NotEmpty())
        {
            query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient) ||
                                          recipe.Ingredients.Any(ingredient =>
                                              ingredient.Item.Contains(filters.RecipeTitle_Ingredient)));
        }

        return await query.ToListAsync();
        
    }

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, int recipeId)
    {
        return await  GetFullRecipe().AsNoTracking().FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public async Task<IList<Recipe>> GetForDashboard(User user)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(c => c.Ingredients)
            .Where(c => c.Active && c.UserId == user.Id)
            .OrderByDescending(r => r.CreatedOn)
            .Take(5)
            .ToListAsync();
    }


    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, int recipeId)
    {
        return await  GetFullRecipe().FirstOrDefaultAsync(recipe => recipe.Active && recipe.Id == recipeId && recipe.UserId == user.Id);
    }

    public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);



    private IIncludableQueryable<Recipe, IList<DishType>?> GetFullRecipe()
    {
        return _dbContext.Recipes
            .Include(recipe => recipe.Ingredients)
            .Include(recipe => recipe.Instructions)
            .Include(recipe => recipe.DishTypes);
    }
}