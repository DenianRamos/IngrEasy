using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Communication.Requests;

namespace WebApi.Test.User.ChangePasswordTest;

public class ChangePasswordTest : IngrEasyClassFixture
{
    private const string METHOD = "user/change-password";
    private readonly string _email;
    private readonly string _password;
    private readonly Guid _userIdentifier;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _userIdentifier = factory.GetUserIdentifier();
    }


    [Fact]
    public async Task Sucess()
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Password = _password;
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut(METHOD, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };
        
        response = await DoPost("login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        

        loginRequest.Password = request.NewPassword;
        
        response = await DoPost("login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
    }
}