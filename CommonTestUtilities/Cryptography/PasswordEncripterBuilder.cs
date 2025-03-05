using IngrEasy.Domain.Security.Criptography;
using IngrEasy.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncrypter Build() => new Sha512Encrypter("abcv");

}