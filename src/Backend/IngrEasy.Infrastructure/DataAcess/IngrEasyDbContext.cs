using IngrEasy.Domain;
using Microsoft.EntityFrameworkCore;

namespace IngrEasy.Infrastructure.DataAcess;

public class IngrEasyDbContext : DbContext
{
    public IngrEasyDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IngrEasyDbContext).Assembly);
    }
}