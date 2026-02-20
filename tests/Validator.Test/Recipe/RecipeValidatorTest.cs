using System.Diagnostics.CodeAnalysis;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe;
using IngrEasy.Communication.Enums;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;

namespace Validator.tests.Recipe;

public class RecipeValidatorTest
{
    [Fact]

    public void Sucess()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Error_Invalid_CookingTime()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.CookingTime = (CookingTime)100;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.COOKING_TIME_NOT_SUPPORTED);
    }
    
    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.Difficulty = (Difficulty)100;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.DIFFICULTY_LEVEL_NOT_SUPPORTED);

    }
    
    [Theory]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public void Error_Invalid_Title_CookingTime(string title)
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.Title = title;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.TITLE_EMPTY);
    }


    [Fact]

    public void Sucess_CookingTime_Null()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.CookingTime = null;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Success_DishTypes_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Clear();

        var validator = new RecipeValidator();
        
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes.Add((DishType)100);
        

        var validator = new RecipeValidator();
        
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.DISH_TYPE_NOT_SUPPORTED));
    }
    
    [Fact]
    public void Error_Empty_Ingredients()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.Ingredients.Clear();
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.INGREDIENTS_MINIMUM);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("              ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]

    public void Error_Empty_Value_Ingredient(string ingredient)
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();
        
        request.Ingredients.Add(ingredient);
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.INGREDIENT_EMPTY);
    }
    
    
    [Fact]
    public void Error_Same_Instructions_Step()
    {
        var validator = new RecipeValidator();
        
        var request = RequestRecipeJsonBuilder.Build();

        request.Instructions.First().Step = request.Instructions.Last().Step;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage == ResourceErrorMessage.DUPLICATED_INSTRUCTIONS);
    }

    [Fact]
    public void Error_Instruction_Too_Long()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);
        
        var validator = new RecipeValidator();
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.INSTRUCTION_TEXT_MAX_LENGTH));

    }
    
    
    
}