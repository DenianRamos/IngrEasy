using IngrEasy.Communication.Enums;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Services.OpenAI;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.Generate;

public class GenerateRecipeUseCase : IGenerateRecipeUseCase
{
    private readonly IGenerateRecipeAI _generator;

    public GenerateRecipeUseCase(IGenerateRecipeAI generator)
    {
        _generator = generator;
    }

    public async Task<ResponseGeneratedRecipeJson> Execute(RequestGeneratedRecipeJson request)
    {
        Validate(request);
        var response = await _generator.Generate(request.Ingredients);
        
        return new ResponseGeneratedRecipeJson
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            CookingTime = (CookingTime)response.CookingTime,
            Instructions = response.Instructions.Select(c => new ResponseGeneratedInstructionJson
            {
                Step = c.Step,
                Text = c.Text,
            }).ToList(),
            Difficulty = Difficulty.Low
        };
    }
    
    
    public static void Validate(RequestGeneratedRecipeJson request)
    {
        var result = new GenerateRecipeValidator().Validate(request);
        
        if (result.IsValid.IsFalse())
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}