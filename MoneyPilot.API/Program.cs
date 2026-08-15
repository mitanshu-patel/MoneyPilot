using Microsoft.EntityFrameworkCore;
using MoneyPilot.API;
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
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

builder.Services.DefaultRegistrations(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Value ?? "";
        policy.WithOrigins(allowedOrigins.Split(";", StringSplitOptions.RemoveEmptyEntries)).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 2. Map the OpenAPI JSON endpoint (/openapi/v1.json)
    app.MapOpenApi();

    // 3. Map the interactive UI (Scalar UI at /scalar/v1)
    app.MapScalarApiReference();
}

app.UseCors();
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
