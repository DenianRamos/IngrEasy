using CommonTestUtilities.Entities;
using IngrEasy.Domain;
using IngrEasy.Infrastructure.DataAcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    private  User _user = default!;

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


    private void StartDataBase(IngrEasyDbContext dbContext)
    {
        (_user,_password)  = UserBuilder.Build();

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}