using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.Update;
using IngrEasy.Communication.Requests;
using IngrEasy.Domain.Extensions;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]

    public async Task Sucess()
    {
        (var user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = await CreateUseCase(user);
        
       Func<Task> act = async () =>
       {
           await useCase.Execute(request);
       };

       await act.Should().NotThrowAsync();
       
       user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = await CreateUseCase(user);
        
        Func<Task> act = async () =>
        {
            await useCase.Execute(request);
        };
        
        await act.Should().ThrowAsync<ErrorOnValidationException>().
            Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.NAME_EMPTY));
       
       user.Name.Should().NotBe(request.Name);
       user.Email.Should().NotBe(request.Email);
       
    }
    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        (var user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = await CreateUseCase(user,request.Email);
        
        Func<Task> act = async () =>
        {
            await useCase.Execute(request);
        };
        
        await act.Should().ThrowAsync<ErrorOnValidationException>().
            Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.EMAIL_ALREADY_EXIST));
       
        user.Name.Should().NotBe(request.Name);
        user.Email.Should().NotBe(request.Email);
       
    }



    private async Task<UpdateUseCase> CreateUseCase(IngrEasy.Domain.User user, string email = null)
    {
        var unitofWork = UnitOfWorkBuilder.Build();
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        if (string.IsNullOrWhiteSpace(email).IsFalse())
        {
            userReadOnlyRepository.ExistActiveUserWithEmail(email);
        }
        
        return new UpdateUseCase(unitofWork, userReadOnlyRepository.Build(), loggedUser, userUpdateOnlyRepository);
    }
}