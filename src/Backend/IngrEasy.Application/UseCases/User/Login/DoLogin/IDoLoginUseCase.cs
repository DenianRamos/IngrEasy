using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.User.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}