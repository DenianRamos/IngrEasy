using IngrEasy.Application.UseCases.User.Login;
using IngrEasy.Application.UseCases.User.Login.DoLogin;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace IngrEasy.API.Controllers;
[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]

    public async  Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
       var response = await useCase.Execute(request);
         return Ok(response);
    }
}