using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Enums;

namespace CongestionTaxCalculator.Application.Repositories;

public interface ICityRepository
{
    public Task<City?> GetCityAsync(CityName cityName, CancellationToken cancellationToken);
}
