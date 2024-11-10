using CongestionTaxCalculator.Application.Repositories;
using CongestionTaxCalculator.Infrastructure.Persistence;
using CongestionTaxCalculator.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddDbContext<InMemoryDbContext>(options => options.UseInMemoryDatabase("CongestionTaxCalculator"));

        services.AddScoped<DbContextSeedData>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }
}
