namespace IngrEasy.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    public Task Add(Domain.User user);
}