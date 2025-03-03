using IngrEasy.Domain.Security.Tokens;

namespace IngrEasy.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly HttpContextAccessor _httpContextAccessor;

    public HttpContextTokenValue(HttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Value()
    {
       var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
       
       return authorization["Bearer ".Length..].Trim();

    }
    
}