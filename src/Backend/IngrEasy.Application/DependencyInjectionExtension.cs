using AutoMapper;
using IngrEasy.Application.Services.AutoMapper;
using IngrEasy.Application.Services.Cryptography;
using IngrEasy.Application.UseCases.User.Login;
using IngrEasy.Application.UseCases.User.Login.DoLogin;
using IngrEasy.Application.UseCases.User.Profile;
using IngrEasy.Application.UseCases.User.Register;
using IngrEasy.Application.UseCases.User.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace IngrEasy.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddUseCases(services);
        AddPasswordEncrypter(services, configuration);
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddScoped(opt => new MapperConfiguration(opt =>
        {
            opt.AddProfile(new AutoMapping());
        }).CreateMapper());
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUseUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUsecase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUseCase, UpdateUseCase>();
    }
    
    
    private static void AddPasswordEncrypter(this IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        services.AddScoped(opt => new PasswordEncripter(additionalKey!));
    }
}