using CommonTestUtilities.Dtos;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Generate;
using IngrEasy.Communication.Enums;
using IngrEasy.Domain.Dtos;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.Recipe;

public class GenerateRecipeUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();
        var request = RequestGeneratedRecipeJsonBuilder.Build();
        var useCase = CreateUsecase(dto);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);
        result.CookingTime.Should().Be((CookingTime)dto.CookingTime);
        result.Difficulty.Should().Be(Difficulty.Low); 
    }
    [Fact]
    public async Task Error_Duplicated_Ingredient()
    {
        var dto = GeneratedRecipeDtoBuilder.Build();
        var request = RequestGeneratedRecipeJsonBuilder.Build(4);
        request.Ingredients.Add(request.Ingredients[0]);
        var useCase = CreateUsecase(dto);
        var act = async () => await useCase.Execute(request);

       await act.Should().ThrowAsync<ErrorOnValidationException>().Where(e =>
            e.ErrorMessage.Count == 1 && e.ErrorMessage.Contains(ResourceErrorMessage.DUPLICATED_INGREDIENTS_IN_lIST));
    }

    private static GenerateRecipeUseCase CreateUsecase(GeneratedRecipeDto dto)
    {
        var generateRecipeAI = GenerateRecipeAIBuilder.Build(dto);
        return new GenerateRecipeUseCase(generateRecipeAI);
    }
}