using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IngrEasy.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace IngrEasy.Infrastructure;

public class JwtTokenGenerator :IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;

    public JwtTokenGenerator(string signingKey, uint expirationTimeMinutes)
    {
        _signingKey = signingKey;
        _expirationTimeMinutes = expirationTimeMinutes;
    }

    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }


    private SymmetricSecurityKey SecurityKey()
    {
        var bytes = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}