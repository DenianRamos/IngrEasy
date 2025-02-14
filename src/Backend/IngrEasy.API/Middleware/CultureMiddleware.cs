using System.Globalization;
using IngrEasy.Domain.Extensions;

namespace IngrEasy.API.Middleware;

public class CultureMiddleware
{ 
    private readonly RequestDelegate _next;
    
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        
        var cultureInfo = new CultureInfo("en");

        if (requestCulture.EmptyString() && supportedLanguages.Exists(x => x.Name == requestCulture))
            cultureInfo = new CultureInfo(requestCulture);
        
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        
        await _next(context);
        
    }
}