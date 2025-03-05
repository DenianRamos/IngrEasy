using IngrEasy.API.Converters;
using IngrEasy.API.Filters;
using IngrEasy.API.Middleware;
using IngrEasy.API.Token;
using IngrEasy.Application;
using IngrEasy.Domain.Security.Tokens;
using IngrEasy.Infrastructure;
using IngrEasy.Infrastructure.Extensions;
using IngrEasy.Infrastructure.Migrations;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Jwt Authorization header using the Bearer scheme.
                        Enter 'Bearer [space]' and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
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
