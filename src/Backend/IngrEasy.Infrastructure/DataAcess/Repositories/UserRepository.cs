using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace IngrEasy.Infrastructure.DataAcess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly IngrEasyDbContext _dbContext; 

    public UserRepository(IngrEasyDbContext dbContext) =>   _dbContext = dbContext;
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user); 
    
    public async Task<bool> ExistActiveUserByEmail(string email) => await _dbContext.Users.AnyAsync(x => x.Email == email && x.Active);
    


}