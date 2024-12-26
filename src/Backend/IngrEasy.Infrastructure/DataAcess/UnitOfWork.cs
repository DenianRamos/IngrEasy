using IngrEasy.Domain;

namespace IngrEasy.Infrastructure.DataAcess;

public class UnitOfWork : IUnitOfWork
{
    private readonly IngrEasyDbContext _dbContext;

    public UnitOfWork(IngrEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }
}