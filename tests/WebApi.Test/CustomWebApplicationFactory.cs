using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncryption;
using IngrEasy.Communication.Enums;
using IngrEasy.Domain;
using IngrEasy.Infrastructure.DataAcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CookingTime = IngrEasy.Domain.Enum.CookingTime;
using Difficulty = IngrEasy.Domain.Enum.Difficulty;
using DishType = IngrEasy.Domain.Enum.DishType;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    private  IngrEasy.Domain.Entities.User _user = default!;
    
    private IngrEasy.Domain.Entities.Recipe _recipe = default!;

    private string _password = string.Empty;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test").ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<IngrEasyDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<IngrEasyDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(provider);
            });

           using var scope = services.BuildServiceProvider().CreateScope();
           var dbContext = scope.ServiceProvider.GetRequiredService<IngrEasyDbContext>();

           dbContext.Database.EnsureDeleted();

            StartDataBase(dbContext);

        });
        
    }

    public string GetEmail() => _user.Email;
    
    public string GetPassword() => _password;
    
    public string GetName() => _user.Name;
    
    public Guid GetUserIdentifier() => _user.UserIdentifier;
    
    
    public string GetRecipeTitle() => _recipe.Title;

    public string GetRecipeId() => IdEncripterBuilder.Build().Encode(_recipe.Id);
    
    public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
    public CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
    
    public IList<DishType> GetRecipeDishTypes() => _recipe.DishTypes!.Select(c => c.Type).ToList();
    
    


    private void StartDataBase(IngrEasyDbContext dbContext)
    {
        (_user,_password)  = UserBuilder.Build();

        _recipe = RecipeBuilder.Build(_user);
        dbContext.Recipes.Add(_recipe);
        

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}