using IngrEasy.API.Converters;
using IngrEasy.API.Filters;
using IngrEasy.API.Middleware;
using IngrEasy.Application;
using IngrEasy.Infrastructure;
using IngrEasy.Infrastructure.Extensions;
using IngrEasy.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CultureMiddleware>();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDataBase(builder.Configuration);
app.Run();

void MigrateDataBase(IConfiguration configuration)
{
    if (configuration.IsTestEnvironment())
        return;
    
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var connectionString = configuration.AddConnectionString();
    DataBaseMigration.Migrate(connectionString!, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program()
    {
        
    }
}
