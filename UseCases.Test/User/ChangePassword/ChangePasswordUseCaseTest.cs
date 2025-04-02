using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.ChangePassword;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]

    public async Task Sucess()
    {
        (var user, var password) =  UserBuilder.Build();
        
        var request = RequestChangePasswordBuilder.Build();
        request.Password = password;
    
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);
        await act.Should().NotThrowAsync();
        
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        
        user.Password.Should().Be(passwordEncrypter.Encrypt(request.NewPassword));
    }
    
    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        (var user, var password) =  UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            NewPassword = string.Empty,
            Password = password
        };
        
    
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidationException>().Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.PASSWORD_EMPTY));
        
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        
        user.Password.Should().Be(passwordEncrypter.Encrypt(password));
    }

    [Fact]
    public async Task Error_CurrentPassword_Diferent()
    {
        (var user, var password) =  UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            NewPassword = string.Empty,
            Password = password
        };
        
    
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);
        await act.Should().ThrowAsync<ErrorOnValidationException>().Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.PASSWORD_EMPTY));
        
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        
        user.Password.Should().Be(passwordEncrypter.Encrypt(password));
    }


    private static ChangePasswordUseCase CreateUseCase(IngrEasy.Domain.User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateUserOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        
        return new ChangePasswordUseCase(loggedUser, unitOfWork, updateUserOnlyRepository, passwordEncrypter);
    }
}
