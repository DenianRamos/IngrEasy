using System.Data;
using FluentValidation;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;

namespace IngrEasy.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage(ResourceErrorMessage.TITLE_EMPTY);
        RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage(ResourceErrorMessage.COOKING_TIME_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage(ResourceErrorMessage.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage(ResourceErrorMessage.INGREDIENTS_MINIMUM);
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage(ResourceErrorMessage.INSTRUCTIONS_MINIMUM);
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage(ResourceErrorMessage.DISH_TYPE_NOT_SUPPORTED);
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage(ResourceErrorMessage.INGREDIENT_EMPTY);
        RuleForEach(recipe => recipe.Instructions).ChildRules(instruction =>
        {
            instruction.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage(ResourceErrorMessage.STEP_GREATER_THAN_ZERO);
            instruction.RuleFor(instruction => instruction.Text)
                .NotEmpty().WithMessage(ResourceErrorMessage.INSTRUCTION_TEXT_EMPTY)
                .MaximumLength(2000).WithMessage(ResourceErrorMessage.INSTRUCTION_TEXT_MAX_LENGTH);
        });
        RuleFor(recipe => recipe.Instructions).Must(instruction =>
            instruction.Select(i => i.Step).Distinct().Count() == instruction.Count).WithMessage(ResourceErrorMessage.DUPLICATED_INSTRUCTIONS);
    }
}