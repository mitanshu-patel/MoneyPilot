using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
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
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace MoneyPilot.API
{
    public static class MoneyPilotRegistrations
    {
        public static void DefaultRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            var appInsightsConnection = configuration.GetConfigurationValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");
            var environment = configuration.GetConfigurationValue<string>("ENVIRONMENT");
            var isDevelopment = !string.IsNullOrEmpty(environment) && environment.Equals(
                        "DEVELOPMENT",
                        System.StringComparison.InvariantCultureIgnoreCase);

            services.AddScoped<IMediator, Mediator>();
            services.RegisterHandlers(Assembly.Load("MoneyPilot.Application"));
            var connectionString = ConnectionStringsHelper.GetDbConnectionString();
            services.AddDbContext<MoneyPilotContext>(
                v => v.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("MoneyPilot.Infrastructure")));
            services.AddScoped<IMoneyPilotRepo, MoneyPilotRepo>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IEncryptionDecryptionService, EncryptionDecryptionService>();
            services.Configure<EncryptionDecryptionServiceOptions>(configuration.GetSection("Encryption"));

            services
           .AddOpenTelemetry()
           .ConfigureResource(v => v.AddService("MoneyPilot"))
           .WithTracing(builder =>
           {
               builder
                   .AddSource("Microsoft.SemanticKernel*")
                   .AddHttpClientInstrumentation()
                   .AddAspNetCoreInstrumentation()
                   .AddEntityFrameworkCoreInstrumentation()
                   .AddAzureMonitorTraceExporter(o =>
                   {
                       o.ConnectionString = appInsightsConnection;
                   });
           });

            services.AddLogging(v =>
            {
                if (isDevelopment)
                {
                    v.AddConsole();
                }
                v.AddOpenTelemetry(options =>
                {
                    options.AddAzureMonitorLogExporter(o => o.ConnectionString = appInsightsConnection);

                    options.IncludeFormattedMessage = true;
                    options.IncludeScopes = true;
                });
            });

            // Add services to the container.
            services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keeps PascalCase
            });

            services.AddEndpointsApiExplorer();

            services.AddOpenApi(options =>
            {
                // Define the Bearer security scheme globally
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        BearerFormat = "JWT"
                    };

                    return Task.CompletedTask;
                });

                // Apply security requirement only to operations with CustomAuthorizeFilter
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var hasCustomAuthorize = context.Description.ActionDescriptor.FilterDescriptors
                        .Any(fd => fd.Filter is Microsoft.AspNetCore.Mvc.TypeFilterAttribute tfa &&
                                   tfa.ImplementationType?.Name == "CustomAuthorizeFilter");

                    if (hasCustomAuthorize)
                    {
                        operation.Security ??= new List<OpenApiSecurityRequirement>();
                        operation.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", null)] = []
                        });
                    }

                    return Task.CompletedTask;
                });
            });
        }
    }
}
