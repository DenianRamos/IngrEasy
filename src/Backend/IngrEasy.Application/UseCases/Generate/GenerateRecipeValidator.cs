using System.Data;
using FluentValidation;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.ValueObjects;
using IngrEasy.Exception;

namespace IngrEasy.Application.UseCases.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGeneratedRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maximumNumbersIngredients = IngrEasyRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;
        
        RuleFor(request => request.Ingredients.Count).InclusiveBetween(1,maximumNumbersIngredients).WithMessage(ResourceErrorMessage.INVALID_NUMBER_INGREDIENTS);

        RuleFor(request => request.Ingredients)
            .Must(ingredients => ingredients.Count == ingredients.Distinct().Count())
            .WithMessage(ResourceErrorMessage.DUPLICATED_INGREDIENTS_IN_lIST);

        RuleFor(request => request.Ingredients).ForEach(rule =>
        {
            rule.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    context.AddFailure("Ingredient", ResourceErrorMessage.INGREDIENT_EMPTY);
                    return;
                }
        
                if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                {
                    context.AddFailure("Ingredient", ResourceErrorMessage.INGREDIENT_INVALID_FORMAT);
                    return;
                }
            });
        });
    }
}