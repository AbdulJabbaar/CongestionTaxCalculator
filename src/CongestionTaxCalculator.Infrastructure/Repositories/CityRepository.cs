using CongestionTaxCalculator.Application.Repositories;
using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Enums;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly InMemoryDbContext _dbContext;

    public CityRepository(InMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<City?> GetCityAsync(CityName cityName, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.FirstOrDefaultAsync(x => x.Name == cityName, cancellationToken);
    }
}
