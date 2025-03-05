namespace IngrEasy.Domain.Security.Criptography;

public interface IPasswordEncrypter
{
    public string Encrypt(string password);
}