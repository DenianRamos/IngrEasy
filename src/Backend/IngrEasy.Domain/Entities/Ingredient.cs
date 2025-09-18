using System.ComponentModel.DataAnnotations.Schema;
using IngrEasy.Domain.Entities;

namespace IngrEasy.Domain;


[Table("Ingredients")]
public class Ingredient : EntityBase
{
    public string Item { get; set; } = string.Empty;
    
    public int RecipeId { get; set; }
    
}