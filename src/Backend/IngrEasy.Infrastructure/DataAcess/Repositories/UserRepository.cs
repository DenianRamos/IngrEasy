using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace IngrEasy.Infrastructure.DataAcess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUpdateUserOnlyRepository
{
    private readonly IngrEasyDbContext _dbContext; 

    public UserRepository(IngrEasyDbContext dbContext) =>   _dbContext = dbContext;
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user); 
    
    public async Task<bool> ExistActiveUserByEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email == email && user.Active);
    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active && user.Password.Equals(password));
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

    public Task<User> GetById(long id)
    {
        return _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user) => _dbContext.Users.Update(user);


}