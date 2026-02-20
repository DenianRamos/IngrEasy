using IngrEasy.Communication.Enums;

namespace IngrEasy.Communication.Response;

public class ResponseGeneratedRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public IList<string> Ingredients { get; set; } = [];
    public CookingTime? CookingTime { get; set; }
    public IList<ResponseGeneratedInstructionJson> Instructions { get; set; } = [];
    public Difficulty? Difficulty { get; set; }
    

}