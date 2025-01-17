using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace IngrEasy.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static void Migrate(string connectionString,IServiceProvider serviceProvider)
    {
        EnsureDatabaseCreated(connectionString);
        MigrationDataBase(serviceProvider);
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


    private static void MigrationDataBase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.ListMigrations();
        runner.MigrateUp();
        
        
    }
}