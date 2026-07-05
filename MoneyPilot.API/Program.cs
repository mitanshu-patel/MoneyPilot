using Microsoft.EntityFrameworkCore;
using MoneyPilot.Application;
using MoneyPilot.Application.Services;
using MoneyPilot.Infrastructure;
using MoneyPilot.Infrastructure.Repos;
using MoneyPilot.Infrastructure.Services;
using MoneyPilot.Shared.Common;
using MoneyPilot.Shared.Contracts;
using MoneyPilot.Shared.EncryptionDecryption;
using MoneyPilot.Shared.Helpers;
using MoneyPilot.Shared.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
var environment = configuration.GetValue<string>("ENVIRONMENT");
var isDevelopment = !string.IsNullOrEmpty(environment) && environment.Equals(
            "DEVELOPMENT",
            System.StringComparison.InvariantCultureIgnoreCase);

builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.RegisterHandlers(Assembly.Load("MoneyPilot.Application"));
var connectionString = ConnectionStringsHelper.GetDbConnectionString();
builder.Services.AddDbContext<MoneyPilotContext>(
    v => v.UseSqlServer(connectionString,
    b => b.MigrationsAssembly("MoneyPilot.Infrastructure")));
builder.Services.AddScoped<IMoneyPilotRepo, MoneyPilotRepo>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IEncryptionDecryptionService, EncryptionDecryptionService>();
builder.Services.Configure<EncryptionDecryptionServiceOptions>(configuration.GetSection("Encryption"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (builder.Configuration.GetValue<bool>("RunMigration"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MoneyPilotContext>();
    db.Database.Migrate();
    //return; // Exit after migration
}

app.Run();
