using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe.Delete;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.Recipe;

public class DeleteRecipeUseCaseTest
{


    [Fact]

    public async Task Sucess()
    {
        (var user,_) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user: user);

        var useCase = CreateUseCase(user,recipe);

        Func<Task> act = async () => await useCase.Execute(recipe.Id);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Recipe_Not_Found()
    {
        (var user,_) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(1000);

        await act.Should().ThrowAsync<NotFoundException>()
            .Where(e => e.Message.Equals(ResourceErrorMessage.RECIPE_NOT_FOUND));
    }
    private static DeleteRecipeUseCase CreateUseCase(IngrEasy.Domain.Entities.User user, IngrEasy.Domain.Entities.Recipe recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new RecipeReadOnlyRepositoryBuilder().GetById(user,recipe).Build();
        var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new DeleteRecipeUseCase(loggedUser, unitOfWork, repositoryBuilder, writeOnlyRepository);

    }
}