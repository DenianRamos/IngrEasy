namespace IngrEasy.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public  Task<bool> ExistActiveUserByEmail(string email);
}
