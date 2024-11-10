using CongestionTaxCalculator.API.Middlewares;
using CongestionTaxCalculator.Application;
using CongestionTaxCalculator.Infrastructure;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services
    .AddScoped<ExceptionMiddleware>()
    .AddApplicationLayer()
    .AddInfrastructureLayer()
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
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

//Add support to logging request with SERILOG
app
    .UseSerilogRequestLogging()
    .UseMiddleware<ExceptionMiddleware>();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<DbContextSeedData>();
await service.SeedAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }