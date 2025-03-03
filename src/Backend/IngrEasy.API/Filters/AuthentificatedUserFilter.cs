using IngrEasy.Communication.Response;
using IngrEasy.Domain.Extensions;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace IngrEasy.API.Filters;

public class AuthentificatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAcessTokenValidator _acessTokenValidator;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public AuthentificatedUserFilter(IAcessTokenValidator acessTokenValidator, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _acessTokenValidator = acessTokenValidator;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
        
            var userIdentifier = _acessTokenValidator.ValidateAndGetUserIdentifier(token);
            var exist = await _userReadOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);

            if (exist.IsFalse())
            {
                throw new IngrEasyException(ResourceErrorMessage.USER_WITHOUT_PERMISSON_ACCESS_RESOURCE);
            }
        }
        catch (SecurityTokenExpiredException )
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
            {
                TokenExpired = true
            });
        }
        catch (IngrEasyException e)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(e.Message));
        }
        catch 
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceErrorMessage.USER_WITHOUT_PERMISSON_ACCESS_RESOURCE));
        }
    }
    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentification = context.HttpContext.Request.Headers.Authorization.ToString();


        if (string.IsNullOrWhiteSpace(authentification))
        {
            throw new IngrEasyException(ResourceErrorMessage.NO_TOKEN);
        }
       
        return authentification["Bearer ".Length..].Trim();
    }
}