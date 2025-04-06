namespace IngrEasy.Domain;

public class Ingredient : EntityBase
{
    public string Item { get; set; } = string.Empty;
    
    public int RecipeId { get; set; }
    
}