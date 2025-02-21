using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    public void ExistActiveUserWithEmail(string email)
    {
        _mock.Setup(repository => repository.ExistActiveUserByEmail(email)).ReturnsAsync(true);
    }
    public void GetByEmailAndPassword(User user)
    {
        _mock.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
    }
    
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public UserReadOnlyRepositoryBuilder() => _mock = new Mock<IUserReadOnlyRepository>();


    public  IUserReadOnlyRepository Build() => _mock.Object;



}