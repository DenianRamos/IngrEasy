using IngrEasy.Domain.Enum;

namespace IngrEasy.Domain.Dtos;

public record FilterRecipesDto
{
    public string? RecipeTitle_Ingredient { get; set; } = string.Empty;
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];

}