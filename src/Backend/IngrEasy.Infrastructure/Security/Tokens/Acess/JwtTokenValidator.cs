using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IngrEasy.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace IngrEasy.Infrastructure.Security.Tokens.Acess;

public class JwtTokenValidator : JwtTokenHandler, IAcessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey)
    {
        _signingKey = signingKey;
    }
    
    public Guid ValidateAndGetUserIdentifier(string token)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ClockSkew = new TimeSpan(0),
            ValidateAudience = false,
            IssuerSigningKey = CreateSecurityKey(_signingKey)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
        
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        return Guid.Parse(userIdentifier);

    }
}