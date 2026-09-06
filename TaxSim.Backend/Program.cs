using TaxSim.Backend.Data;
using TaxSim.Backend.Models;
using TaxSim.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddSwaggerGen(); // Add this line

builder.Services.AddDbContext<TaxSimDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TaxSimulationService>();

builder.Services.AddScoped<ILLMClient, OllamaLLMClient>();
builder.Services.AddScoped<IAgentAiService, AgentAiService>();
builder.Services.AddScoped<TaxSimulationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.MapControllers();

// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaxSimDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.TaxPolicies.Any())
    {
        dbContext.TaxPolicies.Add(new TaxPolicy {
            PolicyTitle = "Standard Tax Policy",
            NaturalLanguageRule = "All citizens must pay 15% of their income as tax."
        });
        dbContext.SaveChanges();
    }

    if (!dbContext.TaxAgents.Any())
    {
        dbContext.TaxAgents.AddRange(
            new TaxAgent { AgentName = "Alice H", BehavioralProfile = "Compliant", Income = 120000m },
            new TaxAgent { AgentName = "Bob J", BehavioralProfile = "Aggressive", Income = 500000m },
            new TaxAgent { AgentName = "Charlie C", BehavioralProfile = "Corporate", Income = 1200000m },
            new TaxAgent { AgentName = "Diana R", BehavioralProfile = "Conservative", Income = 85000m }
        );
        dbContext.SaveChanges();
    }

    var simulationService = scope.ServiceProvider.GetRequiredService<TaxSimulationService>();
    var result = await simulationService.RunSimulationAsync(1);
    
    Console.WriteLine("========================================");
    Console.WriteLine("SIMULATION TEST SUCCESSFUL FOR POLICY 1");
    Console.WriteLine($"Simulation Run ID: {result.Id}");
    Console.WriteLine($"Total Revenue Collected: {result.TotalRevenueCollected}");
    Console.WriteLine($"Compliance Failures: {result.ComplianceFailureCount}");
    Console.WriteLine($"Audit Logs Count: {result.AuditLogs?.Count ?? 0}");
    Console.WriteLine("========================================");
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}