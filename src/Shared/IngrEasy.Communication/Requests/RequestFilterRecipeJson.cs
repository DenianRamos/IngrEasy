using IngrEasy.Communication.Enums;

namespace IngrEasy.Communication.Requests;

public class RequestFilterRecipeJson
{
    public string RecipeTitle_Ingredient { get; set; } = string.Empty;

    public IList<CookingTime> CookingTimes { get; set; } = [];
    
    public IList<Difficulty> Difficulty { get; set; } = [];
    
    public IList<DishType> DishType { get; set; } = [];
}