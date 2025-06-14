using IngrEasy.API.Attributes;
using IngrEasy.Application.UseCases.User.ChangePassword;
using IngrEasy.Application.UseCases.User.Profile;
using IngrEasy.Application.UseCases.User.Register;
using IngrEasy.Application.UseCases.User.Update;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace IngrEasy.API.Controllers;
[ApiController]
[Route("[controller]")]

public class UserController : IngrEasyController
{
    
    [ProducesResponseType(typeof(RequestRegisterUserJson), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Register([FromServices] IRegisterUseUseCase useCase,[FromBody] RequestRegisterUserJson request)
    {
        var result =  await useCase.Execute(request);
        return Created(string.Empty,result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfileUsecase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> UpdateUserProfile([FromServices]IUpdateUseCase useCase, [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswordProfile([FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }


}