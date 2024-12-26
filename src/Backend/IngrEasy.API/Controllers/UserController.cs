using IngrEasy.Application.UseCases.User.Register;
using IngrEasy.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IngrEasy.API.Controllers;
[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    
    [ProducesResponseType(typeof(RequestRegisterUserJson), StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<IActionResult> Register([FromServices] IRegisterUseUseCase useCase,[FromBody] RequestRegisterUserJson request)
    {
        var result =  await useCase.Execute(request);
        return Created(string.Empty,result);
    }

}