using AutoMapper;
using IngrEasy.Application.Services.AutoMapper;
using IngrEasy.Application.UseCases.Recipe.Filter;
using IngrEasy.Application.UseCases.Recipe.Register;
using IngrEasy.Application.UseCases.User.ChangePassword;
using IngrEasy.Application.UseCases.User.Login;
using IngrEasy.Application.UseCases.User.Login.DoLogin;
using IngrEasy.Application.UseCases.User.Profile;
using IngrEasy.Application.UseCases.User.Register;
using IngrEasy.Application.UseCases.User.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace IngrEasy.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services,configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<int>(new()
        {
        MinLength = 3,
        Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        
        services.AddScoped(opt => new MapperConfiguration(opt =>
        {
            opt.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IRegisterUseUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUsecase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUseCase, UpdateUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
    }
    
}