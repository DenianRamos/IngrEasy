using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.Update;

public class UpdateRecipeTest : IngrEasyClassFixture
{
    private const string METHOD = "recipe";
    private readonly Guid _userIdentifier;
    private readonly string _recipeId;

    public UpdateRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _recipeId = factory.GetRecipeId();
        _userIdentifier = factory.GetUserIdentifier();

    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRecipeJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut($"{METHOD}/{_recipeId}",request,token);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPut($"{METHOD}/{_recipeId}",request,token);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}