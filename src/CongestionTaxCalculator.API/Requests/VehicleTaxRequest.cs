using CongestionTaxCalculator.Domain.City.Enums;

namespace CongestionTaxCalculator.API.Requests
{
    public record CalculateVehicleTaxRequest(CityName CityName, VehicleType VehicleType, DateTime[] DatePassesToll);
}
