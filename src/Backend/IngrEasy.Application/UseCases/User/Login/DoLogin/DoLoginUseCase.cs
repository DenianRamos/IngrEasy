using IngrEasy.Application.Services.Cryptography;
using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Exception.ExceptionBase;

namespace IngrEasy.Application.UseCases.User.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    
    private readonly IUserReadOnlyRepository _repository;
    private readonly PasswordEncripter _passwordEncripter;

    public DoLoginUseCase(PasswordEncripter passwordEncripter, IUserReadOnlyRepository repository)
    {
        _passwordEncripter = passwordEncripter;
        _repository = repository;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
        var user = await _repository.GetByEmailAndPassword(request.Email,encriptedPassword);
        
        if (user is null)
        {
            throw new InvalidLoginException();
        }

        return new ResponseRegisterUserJson
        {
            Name = user.Name
        };

    }
}