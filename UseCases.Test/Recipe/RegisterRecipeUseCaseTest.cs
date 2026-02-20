using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.Recipe.Register;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.Recipe;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNull();
        result.Title.Should().Be(request.Title);

        /*(await.act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.ErrorMessage.Count == 1 && e.ErrorMessage.Contains()))*/


    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();

        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () =>
        {
           await useCase.Execute(request);
        };
        
        await act.Should().ThrowAsync<ErrorOnValidationException>()
            .Where(e => e.ErrorMessage.Count == 1 && e.ErrorMessage.Contains(ResourceErrorMessage.TITLE_EMPTY));
        
    }




    private static RegisterRecipeUseCase CreateUseCase(IngrEasy.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var recipeWriteOnlyRepository =  RecipeWriteOnlyRepositoryBuilder.Build();
        
        return new RegisterRecipeUseCase(recipeWriteOnlyRepository, loggedUser, unitOfWork, mapper);
    }
}