using IngrEasy.Domain.Enum;

namespace IngrEasy.Domain.Dtos;

public class GeneratedRecipeDto
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = new List<string>();
    public IList<GeneratedInstructionDto> Instructions { get; set; } = new List<GeneratedInstructionDto>();
    public CookingTime CookingTime { get; set; }
}