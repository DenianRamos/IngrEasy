using System.Data;
using FluentValidation;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;

namespace IngrEasy.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title).NotEmpty().WithMessage("Titulo Não pode ser vazio");
        RuleFor(recipe => recipe.CookingTime).IsInEnum().WithMessage("Cooking Time Nao suportado");
        RuleFor(recipe => recipe.Difficulty).IsInEnum().WithMessage("Dificuldade Nao suportado");
        RuleFor(recipe => recipe.Ingredients.Count).GreaterThan(0).WithMessage("No minimo um ingrediente");
        RuleFor(recipe => recipe.Instructions.Count).GreaterThan(0).WithMessage("No minimo uma instrução");
        RuleForEach(recipe => recipe.DishTypes).IsInEnum().WithMessage("Dish Type Nao suportado");
        RuleForEach(recipe => recipe.Ingredients).NotEmpty().WithMessage("Ingrediente vazio");
        RuleForEach(recipe => recipe.Instructions).ChildRules(instruction =>
        {
        instruction.RuleFor(instruction => instruction.Step).GreaterThan(0).WithMessage("Passo deve ser maior que 0");
        instruction.RuleFor(instruction => instruction.Text)
            .NotEmpty().WithMessage("Texto não pode ser vazio").MaximumLength(2000).WithMessage("Texto deve ter no maximo 2000 caracteres");
        });
        RuleFor(recipe => recipe.Instructions).Must(instruction =>
            instruction.Select(i => i.Step).Distinct().Count() == instruction.Count).WithMessage("Duas ou mais instruções com a mesma ordem");
    }
}