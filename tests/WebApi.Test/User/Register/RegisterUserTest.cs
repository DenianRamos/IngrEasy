using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IngrEasyClassFixture
{
    

    private readonly string method = "user";
    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var response = await DoPost(method, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);
        
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    
    public async Task Error_Name_empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        
        var response = await DoPost(method:"User", request:request,culture:culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);

        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray().Should();
        
        var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("NAME_EMPTY", new System.Globalization.CultureInfo(culture));
        erros.ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

    }
}