namespace IngrEasy.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}