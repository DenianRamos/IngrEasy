using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.Register;
using IngrEasy.Exception;
using IngrEasy.Exception.ExceptionBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{

    
    
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build(); 
        var useCase = CreateUseCase();
        
        var result =  await useCase.Execute(request);
        
       result.Should().NotBeNull();
       result.Name.Should().Be(request.Name);

    }
    
        [Fact]
    public async Task Email_Already_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.EMAIL_ALREADY_EXIST));
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase();
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.Errors.Count == 1 && e.Errors.Contains(ResourceErrorMessage.NAME_EMPTY));
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitForWork = UnitOfWorkBuilder.Build();
        var useWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncrypter = PasswordEncripterBuilder.Build();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        
        if(string.IsNullOrEmpty(email) == false)
            readRepositoryBuilder.ExistActiveUserWithEmail(email);
        
        return new RegisterUserUseCase(readRepositoryBuilder.Build(),useWriteOnlyRepository,mapper,passwordEncrypter,unitForWork);
    }
    

}