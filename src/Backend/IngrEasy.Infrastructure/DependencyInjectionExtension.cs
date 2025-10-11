using System.Reflection;
using FluentMigrator.Runner;
using IngrEasy.Domain;
using IngrEasy.Domain.Repositories.Recipe;
using IngrEasy.Domain.Repositories.User;
using IngrEasy.Domain.Security.Criptography;
using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Domain.Services.LoggedUser;
using IngrEasy.Infrastructure.DataAcess;
using IngrEasy.Infrastructure.DataAcess.Repositories;
using IngrEasy.Infrastructure.Extensions;
using IngrEasy.Infrastructure.Security.Cryptography;
using IngrEasy.Infrastructure.Security.Tokens.Acess;
using IngrEasy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IngrEasy.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrypter(services,configuration);
        AddRepositories(services);
        AddToken(services,configuration);
        AddLoggedUser(services);
        
        if (configuration.IsTestEnvironment())
            return;

        AddDbContext(services,configuration);
        AddFluentMigrator(services,configuration);

    }

    private static void AddDbContext( IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IngrEasyDbContext>(options =>
        {
            options.UseMySQL(configuration.AddConnectionString()!);
        });
    }

    private static void AddRepositories( IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUpdateUserOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
    }

    private static void AddFluentMigrator( IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddFluentMigratorCore().ConfigureRunner(opt =>
        {
            opt.AddMySql8()
                .WithGlobalConnectionString(connectionString).ScanIn(Assembly.Load("IngrEasy.Infrastructure")).For.All();
        });
    }


    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTime = configuration.GetValue<uint>("Settings:Jwt:ExpirationTime");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        
        services.AddScoped<IAccessTokenGenerator>( option => new JwtTokenGenerator(signingKey!,expirationTime));
        services.AddScoped<IAcessTokenValidator>( option => new JwtTokenValidator(signingKey!));
    }

    private static void AddPasswordEncrypter(this IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        services.AddScoped<IPasswordEncrypter>(opt => new Sha512Encrypter(additionalKey!));
    }
    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();}