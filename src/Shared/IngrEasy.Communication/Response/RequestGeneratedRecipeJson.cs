using IngrEasy.Domain;

namespace IngrEasy.Communication.Response;

public class RequestGeneratedRecipeJson
{
    public IList<string> Ingredients { get; set; } = [];
}