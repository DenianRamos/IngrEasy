using FluentValidation;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;

namespace IngrEasy.Application.UseCases.Recipe.Filter;

public class FilterRecipeValidator : AbstractValidator<RequestsFilterRecipeJson>
{
    public FilterRecipeValidator()
    {
        RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceErrorMessage.COOKING_TIME_NOT_SUPPORTED);
        RuleForEach(r => r.Difficulty).IsInEnum().WithErrorCode(ResourceErrorMessage.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleForEach(r => r.DishType).IsInEnum().WithMessage(ResourceErrorMessage.DISH_TYPE_NOT_SUPPORTED);

    }
}