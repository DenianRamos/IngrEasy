using System.Security.Cryptography;
using System.Text;

namespace IngrEasy.Application.Services.Cryptography;

public class PasswordEncripter
{
    private readonly string _additionalKey;
    public PasswordEncripter( string additionalKey)
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