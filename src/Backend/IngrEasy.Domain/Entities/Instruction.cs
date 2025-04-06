namespace IngrEasy.Domain;

public class Instruction : EntityBase
{
    public int Step { get; set; }
    
    public string Text { get; set; } = string.Empty;
    
    public int RecipeId { get; set; }
}