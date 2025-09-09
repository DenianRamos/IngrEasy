using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe.Filter;
using IngrEasy.Communication.Enums;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;

namespace Validator.tests.Recipe;

public class FilterRecipeValidatorTest
{
    [Fact]
    public void Sucess()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Error_Invalid_CookingTime()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        
        request.CookingTimes.Add((CookingTime)100);
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain( e => e.ErrorMessage.Equals(ResourceErrorMessage.COOKING_TIME_NOT_SUPPORTED));
    }
    
    [Fact]
    public void Error_Invalid_Difficulty()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        
        request.Difficulty.Add((Difficulty)100);
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.DIFFICULTY_LEVEL_NOT_SUPPORTED));
    }
    
    [Fact]
    public void Error_Invalid_DishType()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeJsonBuilder.Build();
        
        request.DishType.Add((DishType)100);
        
        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.DISH_TYPE_NOT_SUPPORTED));
    }
}