using IngrEasy.API.Attributes;
using IngrEasy.API.Binders;
using IngrEasy.Application.UseCases.Generate;
using IngrEasy.Application.UseCases.Recipe.Delete;
using IngrEasy.Application.UseCases.Recipe.Filter;
using IngrEasy.Application.UseCases.Recipe.GetById;
using IngrEasy.Application.UseCases.Recipe.Image;
using IngrEasy.Application.UseCases.Recipe.Register;
using IngrEasy.Application.UseCases.Recipe.Update;
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

    public async Task<IActionResult> Register([FromServices] IRegisterRecipeUseCase useCase,
        [FromBody] RequestRecipeJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpPost("filter")]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Filter([FromServices] IFilterRecipeUseCase useCase,
        [FromBody] RequestFilterRecipeJson request)
    {
        var response = await useCase.Execute(request);
        if (response.Recipes.Any())
        {
            return Ok(response);
        }

        return NoContent();

    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetRecipeByIdUseCase useCase,
        [FromRoute] [ModelBinder(typeof(IngrEasyIdBinder))] int id)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete([FromServices] IDeleteRecipeUseCase useCase,
        [FromRoute] [ModelBinder(typeof(IngrEasyIdBinder))]
        int id)
    {
       await useCase.Execute(id);
       return NoContent();
    }
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateRecipeUseCase useCase,
        [FromRoute] [ModelBinder(typeof(IngrEasyIdBinder))] int id,
        [FromBody] RequestRecipeJson request)
    {
        await useCase.Execute(id, request);
        return NoContent();
    }
    
    [HttpPost("generate")]
    [ProducesResponseType(typeof(ResponseGeneratedRecipeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateRecipe([FromServices] IGenerateRecipeUseCase useCase,[FromBody] RequestGeneratedRecipeJson request)
    {
        {
            var response = await useCase.Execute(request);
            return Ok(response);
        }
    }

    [HttpPut("image/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateImage([FromServices] IAddUpdateImageCoverUseCase useCase,
        [FromRoute] [ModelBinder(typeof(IngrEasyIdBinder))] int id, IFormFile file)
    {
        await useCase.Execute(id, file);
        return NoContent();
    }


}