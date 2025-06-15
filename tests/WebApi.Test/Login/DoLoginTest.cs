using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login;

public class DoLoginTest : IngrEasyClassFixture
{
    private readonly string _method = "login";
    
    
    private readonly string _email;
    
    private readonly string _password;
    
    private readonly string _name;


    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
        

    }

    [Fact]
    public async Task Sucess()
    {
        var request = new RequestLoginJson{Email = _email, Password = _password};
                
        var response = await DoPost(_method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);
        
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]

    public async Task Error_Invalid_user(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        

        
        var response = await  DoPost(method:_method, request:request,culture:culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);

        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray().Should();
        
        var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new System.Globalization.CultureInfo(culture));
        erros.ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    
    
    
    
}