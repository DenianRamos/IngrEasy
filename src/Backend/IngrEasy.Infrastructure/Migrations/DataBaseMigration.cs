using Dapper;
using MySql.Data.MySqlClient;

namespace IngrEasy.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static void Migrate(string connectionString)
    {
        EnsureDatabaseCreated(connectionString);
    }
    
    private static void EnsureDatabaseCreated(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = connectionStringBuilder.Database;
        connectionStringBuilder.Remove("Database");
        var updatedConnectionString = connectionStringBuilder.ToString();
        var parameters = new DynamicParameters();
        parameters.Add("databaseName", databaseName);

        using var dbConnection = new MySqlConnection(updatedConnectionString);
        var records = dbConnection.Query(
            "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @databaseName",
            parameters
        );

        if (!records.Any())
        {
            dbConnection.Execute($"CREATE DATABASE `{databaseName}`;");
        }
    }

}