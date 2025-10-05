using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.GetById;

public class GetByIdRecipeTest : IngrEasyClassFixture
{
    private const string METHOD = "recipe";
    private Guid _userId;
    private readonly string _recipeId;
    private readonly string _recipeTitle;
    public GetByIdRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
        _recipeTitle = factory.GetRecipeTitle();
    }
    
    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var response = await DoGet($"{METHOD}/{_recipeId}", token: token);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().Should().Be(_recipeId);
        responseData.RootElement.GetProperty("title").GetString().Should().Be(_recipeTitle);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var id = IdEncripterBuilder.Build().Encode(1000);
        var response = await DoGet($"{METHOD}/{id}", token: token, culture: culture);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("RECIPE_NOT_FOUND", new System.Globalization.CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain( c => c.GetString()!.Equals(expectedMessage));

    }


}