using IngrEasy.Communication.Response;

namespace IngrEasy.Application.UseCases.User.Profile;

public interface IGetUserProfileUsecase
{
    public Task<ResponseUserProfileJson> Execute();
}