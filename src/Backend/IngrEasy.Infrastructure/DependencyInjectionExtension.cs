using System.Reflection;
using FluentMigrator.Runner;
using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Infrastructure.DataAcess;
using IngrEasy.Infrastructure.DataAcess.Repositories;
using IngrEasy.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IngrEasy.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
        services.AddFluentMigrator(configuration);

    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IngrEasyDbContext>(options =>
        {
            options.UseMySQL(configuration.AddConnectionString()!);
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddFluentMigratorCore().ConfigureRunner(opt =>
        {
            opt.AddMySql8()
                .WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("IngrEasy.Infrastructure")).For.All();
        });
    }

}