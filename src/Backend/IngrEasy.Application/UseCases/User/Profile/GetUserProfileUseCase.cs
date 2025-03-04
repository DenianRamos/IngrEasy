using AutoMapper;
using IngrEasy.Communication.Response;
using IngrEasy.Domain.Services.LoggedUser;

namespace IngrEasy.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUsecase
{

    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}