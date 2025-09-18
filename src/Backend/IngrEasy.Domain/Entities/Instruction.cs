using System.ComponentModel.DataAnnotations.Schema;

namespace IngrEasy.Domain.Entities;

[Table("Instructions")]
public class Instruction : EntityBase
{
    public int Step { get; set; }
    
    public string Text { get; set; } = string.Empty;
    
    public int RecipeId { get; set; }
}