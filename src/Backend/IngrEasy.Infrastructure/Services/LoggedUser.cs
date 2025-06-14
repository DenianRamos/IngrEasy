using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IngrEasy.Domain;
using IngrEasy.Domain.Entities;
using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Infrastructure.DataAcess;
using Microsoft.EntityFrameworkCore;

namespace IngrEasy.Infrastructure.Services;

public class LoggedUser : ILoggedUser
{
    private readonly IngrEasyDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(IngrEasyDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    public async Task<User> User()
    {
        var token = _tokenProvider.Value();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        
        var identifier =jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        var userIdentifier =  Guid.Parse(identifier);
        
        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
        
        
    }
}