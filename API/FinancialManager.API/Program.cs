using FinancialManager.API.HealthChecks;
using FinancialManager.API.Middleware;
using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using FinancialManager.Application.Validators;
using FinancialManager.Domain.Interfaces;
using FinancialManager.Domain.Services;
using FinancialManager.Infrastructure.Persistence;
using FinancialManager.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' não foi configurada.");
}

builder.Services.AddDbContext<FinancialManagerDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Registrar Repositórios (Infrastructure)
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// 3. Registrar Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 4. Registrar Serviços (Application e Domain)
builder.Services.AddScoped<AccountAppService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<TransferAppService>();
builder.Services.AddScoped<CategoryAppService>();
builder.Services.AddScoped<TransferService>();

// 5. Configurar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionRequestValidator>();

// 6. Configurar CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// 7. Configurar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddDbContextCheck<FinancialManagerDbContext>();

// 8. Configurar Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financial Manager API",
        Version = "v1",
        Description = "API para gerenciamento financeiro com suporte a contas, transações, transferências e categorias",
        Contact = new OpenApiContact
        {
            Name = "Financial Manager Team",
            Url = new Uri("https://github.com/VictoRGBC/API-FINANCEIRO")
        }
    });
});

var app = builder.Build();

// 9. Middleware de Tratamento de Exceções Global
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// 10. Configuração do Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial Manager API v1");
    });
}

// Scalar - Interface moderna de documentação
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Financial Manager API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithSidebar(true);
});

// 11. CORS
app.UseCors("ApiPolicy");

// 12. Health Checks Endpoint
app.MapHealthChecks("/health");

// 13. Controllers
app.UseHttpsRedirection();
app.MapControllers();

app.Run();