using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe.Update;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.Recipe;

public class UpdateRecipeUseCaseTest
{
    [Fact]

    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, recipe);
        
        Func<Task> act = async () =>
        {
           await useCase.Execute(recipe.Id, request);
        };
        
        await act.Should().NotThrowAsync();
    }
    
    
    
    [Fact]
    public async Task Error_Recipe_Not_Found()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, recipe);
        Func<Task> act = async () =>
        {
            await useCase.Execute(recipeId:100, request);
        };

        await act.Should().ThrowAsync<IngrEasy.Exception.ExceptionBase.NotFoundException>()
            .Where(e => e.Message.Equals(ResourceErrorMessage.RECIPE_NOT_FOUND));

    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(user, recipe);
        Func<Task> act = async () =>
        {
            await useCase.Execute(recipe.Id, request);
        };

        await act.Should().ThrowAsync<ErrorOnValidationException>().
            Where(e => e.ErrorMessage.Count == 1 && e.ErrorMessage.Contains("Titulo Não pode ser vazio"));
    }


    private static UpdateRecipeUseCase CreateUseCase(IngrEasy.Domain.Entities.User user, IngrEasy.Domain.Entities.Recipe recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user,recipe).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        return new UpdateRecipeUseCase(loggedUser, updateOnlyRepository, unitOfWork, mapper);
    }
}