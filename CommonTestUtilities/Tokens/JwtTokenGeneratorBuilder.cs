using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Infrastructure;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
}