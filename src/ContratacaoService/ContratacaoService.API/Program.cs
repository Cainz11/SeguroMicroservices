using Microsoft.EntityFrameworkCore;
using ContratacaoService.API.Middleware;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Services;
using ContratacaoService.Domain.Repositories;
using ContratacaoService.Infrastructure.Data;
using ContratacaoService.Infrastructure.Repositories;
using ContratacaoService.Infrastructure.Clients;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs - porta 5002 para ContratacaoService
builder.WebHost.UseUrls("http://localhost:5002");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "ContratacaoService API", 
        Version = "v1",
        Description = "API para gerenciamento de contratações de seguro"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContratacaoDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// HTTP Client para comunicação com PropostaService com Polly (resiliência)
builder.Services.AddHttpClient<IPropostaServiceClient, PropostaServiceClient>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

// Dependency Injection - Arquitetura Hexagonal
// Ports (Interfaces) -> Adapters (Implementações)
builder.Services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
builder.Services.AddScoped<IContratacaoService, ContratacaoService.Application.Services.ContratacaoService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ContratacaoDbContext>("database");

var app = builder.Build();

// Middleware de tratamento de exceções
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Desabilitar HTTPS redirection se não estiver em HTTPS
if (app.Environment.IsDevelopment())
{
    // Não redirecionar HTTPS em desenvolvimento
}
else
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
