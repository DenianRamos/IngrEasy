using IngrEasy.Communication.Requests;

namespace IngrEasy.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}