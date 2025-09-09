using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Communication.Requests;
using IngrEasy.Domain.Enum;
using IngrEasy.Exception;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest : IngrEasyClassFixture
{
    private const string METHOD = "recipe/filter";

    private readonly Guid _userIdentifier;
    private string _recipeTitle;
    private Difficulty _recipeDifficulty;
    private CookingTime _recipeCookingTime;
    private IList<DishType> _recipeDishTypes;


    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)

    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeTitle = factory.GetRecipeTitle();
        _recipeDifficulty = factory.GetRecipeDifficulty();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipeDishTypes = factory.GetRecipeDishTypes();
    }

    [Fact]

    public async Task Sucess()
    {
        var request = new RequestFilterRecipeJson
        {   
            CookingTimes = [(IngrEasy.Communication.Enums.CookingTime)_recipeCookingTime],
            Difficulty = [(IngrEasy.Communication.Enums.Difficulty)_recipeDifficulty],
            DishType = _recipeDishTypes.Select(d => (IngrEasy.Communication.Enums.DishType)d).ToList(),
            RecipeTitle_Ingredient = _recipeTitle
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        
        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
        
    }
    
    [Fact]
    public async Task Sucess_NoContent()
    {
        var request =  RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "FilterNoContent";
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPost(method: METHOD, request: request, token: token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }


    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]

    public async Task Error_Cooking_Time(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((IngrEasy.Communication.Enums.CookingTime)100);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage =  ResourceErrorMessage.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new System.Globalization.CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(c => c.GetString() == expectedMessage);
    }
}