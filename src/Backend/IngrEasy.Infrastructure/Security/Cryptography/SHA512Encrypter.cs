using System.Security.Cryptography;
using System.Text;
using IngrEasy.Domain.Security.Criptography;

namespace IngrEasy.Infrastructure.Security.Cryptography;

public class Sha512Encrypter : IPasswordEncrypter
{
    private readonly string _additionalKey;
    public Sha512Encrypter( string additionalKey)
    {
        _additionalKey = additionalKey;
    }
    public string Encrypt(string password)
    {
        var newPassword = password + _additionalKey;
        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hash = SHA512.HashData(bytes);
        
        return StringConverter(hash);
    }

    public static string StringConverter(byte[] bytes)
    {
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            var hax = b.ToString("x2");
            builder.Append(hax);
        }

        return builder.ToString();
    }
}