namespace IngrEasy.Domain.Repositories.User;

public interface IUpdateUserOnlyRepository
{
    public Task<Domain.User> GetById(long id);
    
    public void Update(Domain.User user);
    
}