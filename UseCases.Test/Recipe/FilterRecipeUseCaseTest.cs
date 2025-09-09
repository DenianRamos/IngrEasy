using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe.Filter;
using IngrEasy.Communication.Enums;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;
using IngrEasy.Infrastructure.Services;

namespace UseCases.Test.Recipe;

public class FilterRecipeUseCaseTest
{
    [Fact]

    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestFilterRecipeJsonBuilder.Build();


        var recipes = RecipeBuilder.Collection(user);
        var useCase = CreateUseCase(recipes, user);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Recipes.Should().NotBeNull();
        result.Recipes.Should().HaveCount(recipes.Count);
    }


    [Fact]

    public async Task Error_CookingTime_Invalid()
    {
        (var user, _) = UserBuilder.Build();
        
        var recipes = RecipeBuilder.Collection(user);
        var request = RequestFilterRecipeJsonBuilder.Build();

        request.CookingTimes.Add((IngrEasy.Communication.Enums.CookingTime)100);
        
        var useCase = CreateUseCase(recipes, user);

        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>()).Where(e => e.ErrorMessage.Count == 1 &&
            e.ErrorMessage.Contains(ResourceErrorMessage.COOKING_TIME_NOT_SUPPORTED));
    }


    private static FilterRecipeUseCase CreateUseCase(IList<IngrEasy.Domain.Entities.Recipe> recipe,
        IngrEasy.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().Filter(user, recipe).Build();

        return new FilterRecipeUseCase(mapper, loggedUser, repository);
    }
}