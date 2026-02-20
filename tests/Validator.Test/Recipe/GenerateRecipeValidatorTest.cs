using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Generate;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.ValueObjects;
using IngrEasy.Exception;

namespace Validator.tests.Recipe;

public class GenerateRecipeValidatorTest
{
    [Fact]
    public void Sucess()
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGeneratedRecipeJsonBuilder.Build();
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_More_Maximum_Ingredient()
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGeneratedRecipeJsonBuilder.Build(IngrEasyRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE + 1);
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.INVALID_NUMBER_INGREDIENTS));
    }

    [Fact]
    public void Error_Duplicated_Ingredient()
    {
        var validator = new GenerateRecipeValidator();

        var request = RequestGeneratedRecipeJsonBuilder.Build(4);
        request.Ingredients.Add(request.Ingredients[0]);
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.DUPLICATED_INGREDIENTS_IN_lIST));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Testing null, empty and whitespace strings")]
    public void Error_Empty_Ingredient( string ingredient)
    {
        var validator = new GenerateRecipeValidator();
        var request = RequestGeneratedRecipeJsonBuilder.Build(1);
        request.Ingredients.Add(ingredient);
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.INGREDIENT_EMPTY));
    }

}