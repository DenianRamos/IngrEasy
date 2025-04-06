namespace IngrEasy.Domain;

public class DishType : EntityBase
{
    public Enum.DishType Type { get; set; }
    
    public int RecipeId { get; set; }
}