using IngrEasy.Communication.Requests;

namespace IngrEasy.Application.UseCases.User.Update;

public interface IUpdateUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}