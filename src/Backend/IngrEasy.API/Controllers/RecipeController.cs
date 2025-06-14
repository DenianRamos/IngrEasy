using IngrEasy.API.Attributes;
using IngrEasy.Application.UseCases.Recipe.Filter;
using IngrEasy.Application.UseCases.Recipe.Register;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace IngrEasy.API.Controllers;

[AuthenticatedUser]
public class RecipeController : IngrEasyController
{

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredRecipeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase, [FromBody] RequestRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return CreatedAtAction(nameof(Register), response);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter([FromServices] IFilterRecipeUseCase useCase,
        [FromBody] RequestsFilterRecipeJson request)
    {
        var response = await useCase.Execute(request);
        if (response.Recipes.Any())
        {
            return Ok(response);
        }
        return NoContent();
        
    }
    

}