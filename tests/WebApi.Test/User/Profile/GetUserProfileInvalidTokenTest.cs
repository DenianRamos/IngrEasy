using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Infrastructure;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : IngrEasyClassFixture
{
    private readonly string _method = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {

    }
    
    [Fact]
    public async Task Error_Token_Invalid()
    {
            var response = await DoGet(_method, "TokenIsExpired");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(_method, string.Empty);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Token_With()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        
        var response = await DoGet(_method, token);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}