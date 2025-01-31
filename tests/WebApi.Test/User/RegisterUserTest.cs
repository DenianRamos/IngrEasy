using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Exception;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WebApi.Test.InlineData;

namespace WebApi.Test;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    
    private readonly HttpClient _client;

    public RegisterUserTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var response = await _client.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);
        
        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    
    public async Task Error_Name_empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        if(_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");
        
        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
        
        var response = await _client.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var responseBoby = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBoby);

        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray().Should();
        
        var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("NAME_EMPTY", new System.Globalization.CultureInfo(culture));
        erros.ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

    }
}