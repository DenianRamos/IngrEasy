using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Infrastructure;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : IngrEasyClassFixture
{
    private readonly string _method = "user";

    private readonly string _name;

    private readonly string _email;

    private readonly Guid _userIdentifier;
    
    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _name = factory.GetName();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Sucess()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoGet(_method,token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBoby);
        
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
        responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email);
        

    }
}