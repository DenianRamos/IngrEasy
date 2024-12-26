namespace IngrEasy.Domain;

public interface IUnitOfWork
{
    public Task Commit();
}