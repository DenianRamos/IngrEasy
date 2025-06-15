using IngrEasy.Domain;
using IngrEasy.Domain.Entities;
using IngrEasy.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUpdateUserOnlyRepository> _repository;

    public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUpdateUserOnlyRepository>();
    
    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
      _repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
      return this;
    }
    
    
    public IUpdateUserOnlyRepository Build() => _repository.Object;
}