using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.Login.DoLogin;
using IngrEasy.Communication.Requests;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.User.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {

        var (user,password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        
        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();
        
        var useCase = CreateUseCase();
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>()
            .Where(e => e.Message.Equals(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID));

    }


    private static DoLoginUseCase CreateUseCase(IngrEasy.Domain.User? user = null)
    {
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var jwtTokenGenerator = JwtTokenGeneratorBuilder.Build();
        
        if (user is not null)
            userReadOnlyRepository.GetByEmailAndPassword(user);
        
        return new DoLoginUseCase(passwordEncrypter, userReadOnlyRepository.Build(), jwtTokenGenerator);
    }
}