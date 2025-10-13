using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using IngrEasy.Application.UseCases.Dashboard;

namespace UseCases.Test.Dashboard;

public class GetDashboardUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(user, recipes);
        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Recipes.Should().HaveCountGreaterThan(0)
            .And.OnlyHaveUniqueItems(r => r.Id)
            .And.AllSatisfy(recipe =>
            {
                recipe.Id.Should().NotBeNullOrWhiteSpace();
                recipe.Title.Should().NotBeNullOrWhiteSpace();
                recipe.AmountIngredient.Should().BeGreaterThan(0); 
            });

        }
    
    private static GetDashboardUseCase CreateUseCase(IngrEasy.Domain.Entities.User user, IList<IngrEasy.Domain.Entities.Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var repository = new RecipeReadOnlyRepositoryBuilder().GetForDashBoard(user, recipes).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new GetDashboardUseCase(loggedUser, repository, mapper);
    }
}