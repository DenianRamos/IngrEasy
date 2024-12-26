using IngrEasy.Communication.Requests;
using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.User.Register;

public interface IRegisterUseUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}