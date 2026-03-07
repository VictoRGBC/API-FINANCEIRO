// src/API/Program.cs
using FinancialManager.API.HealthChecks;
using FinancialManager.API.Middleware;
using FinancialManager.Infrastructure.Persistence;
using FinancialManager.Infrastructure.Persistence.Repositories;
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

// 3. Registrar Serviços (Application e Domain)
builder.Services.AddScoped<TransferAppService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<TransferService>(); 

// 4. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. Configurar Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database");

// 6. Configurar Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financial Manager API",
        Version = "v1",
        Description = "API para gerenciamento financeiro com suporte a transações e transferências",
        Contact = new OpenApiContact
        {
            Name = "Financial Manager Team",
            Url = new Uri("https://github.com/VictoRGBC/API-FINANCEIRO")
        }
    });
});

var app = builder.Build();

// 7. Middleware de Tratamento de Exceções Global
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// 8. Configuração do Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial Manager API v1");
    });
}

// Scalar - Interface moderna de documentação (disponível em todos os ambientes)
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Financial Manager API")
        .WithTheme(ScalarTheme.BluePlanet)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
        .WithSidebar(true);
});

// 9. CORS
app.UseCors("AllowAll");

// 10. Health Checks Endpoint
app.MapHealthChecks("/health");

// 11. Controllers
app.UseHttpsRedirection();
app.MapControllers();

app.Run();