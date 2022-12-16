using AuthenticationMicroservice.HealthChecks.DatabaseCheck;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using ProductMicroservice.Models.Request;
using Azure.Security.KeyVault.Secrets;
using ProductMicroservice.Validation;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using HealthChecks.UI.Client;
using Repositories.Abstract;
using Repositories.Context;
using Services.Abstract;
using FluentValidation;
using Azure.Identity;
using Repositories;
using System.Text;
using MassTransit;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add Masstransit && RabbitMQ && Azure Service Bus
/*builder.Services.AddMassTransit(x =>
{
    if (builder.Environment.IsDevelopment())
        x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
    else
        x.UsingAzureServiceBus((context, cfg) => cfg.ConfigureEndpoints(context));
});*/

// Key Vault URL
Environment.SetEnvironmentVariable("KVUrl", "https://applicationkv.vault.azure.net/");

// Database secret connection variables
Environment.SetEnvironmentVariable("TenantId", "b15d7df0-4a92-49e8-b851-b857d77abe6d");
Environment.SetEnvironmentVariable("ClientId", "5ac2fa80-c5b0-4959-8411-ad70a93694d6");
Environment.SetEnvironmentVariable("ClientSecretIdDbConnection", "zXc8Q~rIHIvH.85jBFL5LbtrimP3maTjZPAlOagY");

// JWT secret connection variables
Environment.SetEnvironmentVariable("ClientSecretIdJwt", "rnt8Q~_2L3fIqUenGHJ-tJrtHehzgHPXRcE3Ia_A");

// Connection to Azure Key Vaulte Database connection
var clientDatabase = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KVUrl")!),
                                      new ClientSecretCredential(Environment.GetEnvironmentVariable("TenantId"),
                                                                 Environment.GetEnvironmentVariable("ClientId"),
                                                                 Environment.GetEnvironmentVariable("ClientSecretIdDbConnection")));

// Connection to Azure Key Vaulte Jwt
var clientJwt = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KVUrl")!),
                                 new ClientSecretCredential(Environment.GetEnvironmentVariable("TenantId"),
                                                            Environment.GetEnvironmentVariable("ClientId"),
                                                            Environment.GetEnvironmentVariable("ClientSecretIdJwt")));

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Enable AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddScoped<IValidator<ProductModel>, ProductModelValidator>();

builder.Services.AddEndpointsApiExplorer();

// Add Versioning for swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductMicroservice",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Add Fluent Validation
builder.Services.AddFluentValidationClientsideAdapters();

// Add JWT Bearer validation for Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(clientJwt.GetSecret("Jwt-Secret").Value.Value)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add connection to the databases
builder.Services.AddDbContext<DataContext>(options =>
{
    // Azure connection
    options.UseSqlServer(clientDatabase.GetSecret("ConnectionString-ProductConnection").Value.Value, b => b.MigrationsAssembly("ProductMicroservice"));

    // Local connection
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ProductMicroservice"));
});

// Add Healthcheck
builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck))
                .AddCheck<PingHealthCheck>(nameof(PingHealthCheck));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", config =>
    {
        config.SetIsOriginAllowedToAllowWildcardSubdomains()
              .WithOrigins("http://localhost:4200", "https://applicationclient.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .Build();
    });
});

// Add API Versionings
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(config =>
{
    config.GroupNameFormat = "'v'VVV";
    config.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

// Healthcheck 
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductMicroservice v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
