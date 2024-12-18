using IngrEasy.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IngrEasy.API.Controllers;

[Route("[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    [ProducesResponseType(typeof(RequestRegisterUserJson), StatusCodes.Status201Created)]
    [HttpPost]
    public IActionResult Register()
    {
        return Created();
    }
}