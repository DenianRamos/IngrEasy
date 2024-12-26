using Microsoft.Extensions.Configuration;

namespace IngrEasy.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string? AddConnectionString(this IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("The connection string cannot be null or empty.", nameof(connectionString));
        }
        return connectionString;
    }
}