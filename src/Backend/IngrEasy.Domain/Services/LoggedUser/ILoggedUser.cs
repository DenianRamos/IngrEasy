using IngrEasy.Domain.Entities;

namespace IngrEasy.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}