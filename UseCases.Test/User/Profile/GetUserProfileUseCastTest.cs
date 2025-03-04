using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using FluentAssertions;
using IngrEasy.Application.UseCases.User.Profile;

namespace UseCases.Test.User.Profile;

public class GetUserProfileUseCastTest
{
    [Fact]

    public async Task Sucess()
    {

        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute();
        result.Should().NotBeNull();
        result.Name.Should().NotBeNull(user.Name);
        result.Email.Should().NotBeNull(user.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(IngrEasy.Domain.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggerUser = LoggedUserBuilder.Build(user);
        return new GetUserProfileUseCase(loggerUser, mapper);
    }
}