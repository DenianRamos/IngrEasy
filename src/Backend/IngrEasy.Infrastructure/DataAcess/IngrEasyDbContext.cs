using IngrEasy.Domain;
using IngrEasy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IngrEasy.Infrastructure.DataAcess;

public class IngrEasyDbContext : DbContext
{
    public IngrEasyDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Recipe> Recipes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IngrEasyDbContext).Assembly);
    }
}