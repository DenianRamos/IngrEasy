namespace IngrEasy.Domain.Repositories.User;

public interface IUpdateUserOnlyRepository
{
    public Task<Entities.User> GetById(long id);
    
    public void Update(Entities.User user);
    
}