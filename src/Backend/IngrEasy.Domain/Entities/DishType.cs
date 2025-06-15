using System.ComponentModel.DataAnnotations.Schema;

namespace IngrEasy.Domain.Entities;

[Table("DishTypes")]
public class DishType : EntityBase
{
    public Enum.DishType Type { get; set; }
    
    public int RecipeId { get; set; }
}