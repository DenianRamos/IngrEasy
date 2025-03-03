using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IngrEasy.Infrastructure.Security.Tokens.Acess;

public  abstract class JwtTokenHandler
{
    protected  SymmetricSecurityKey CreateSecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}