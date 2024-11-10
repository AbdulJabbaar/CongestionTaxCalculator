using CongestionTaxCalculator.Domain.City.Enums;
using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;
public class Vehicle : ValueObject
{
    public VehicleType VehicleType { get; private set; }

    private Vehicle(VehicleType vehicleType)
    {
        VehicleType = vehicleType;
    }

    public static Vehicle Create(VehicleType vehicleType)
    {
        return new Vehicle(vehicleType);
    }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return VehicleType;
    }
}

