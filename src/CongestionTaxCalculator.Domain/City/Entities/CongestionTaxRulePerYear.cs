using CongestionTaxCalculator.Domain.City.ValueObjects;
using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.Entities;

public class CongestionTaxRulePerYear : Entity<TaxRulesId>
{
    public int Year { get; private set; }
    public DateTime[] TollFreeDate { get; private set; }
    public int MaximumTollAmountPerDay { get; private set; }
    public int TollDurationMinutes { get; private set; }

    private readonly List<CongestionTaxAmount> _congestionTaxAmounts = [];
    public IReadOnlyList<CongestionTaxAmount> CongestionTaxAmounts => _congestionTaxAmounts.AsReadOnly();

    private readonly List<Vehicle> _tollFreeVehicle = [];
    public IReadOnlyList<Vehicle> TollFreeVehicle => _tollFreeVehicle.AsReadOnly();
    private CongestionTaxRulePerYear() { }
    private CongestionTaxRulePerYear(
        TaxRulesId id,
        int year,
        DateTime[] tollFreeDate,
        int maxTollAmountPerDay,
        int tollDurationMinutes,
        List<CongestionTaxAmount> congestionTaxAmounts,
        List<Vehicle> tollFreeVehicle) : base(id)
    {
        Year = year;
        TollFreeDate = tollFreeDate;
        MaximumTollAmountPerDay = maxTollAmountPerDay;
        TollDurationMinutes = tollDurationMinutes;
        _congestionTaxAmounts = congestionTaxAmounts;
        _tollFreeVehicle = tollFreeVehicle;
    }

    public static CongestionTaxRulePerYear Create(
        TaxRulesId id,
        int year,
        DateTime[] tollFreeDate,
        int maxTollAmountPerDay,
        int tollDurationMinutes,
        List<CongestionTaxAmount> congestionTaxAmounts,
        List<Vehicle> tollFreeVehicle)
    {
        return new CongestionTaxRulePerYear(
            id,
            year,
            tollFreeDate,
            maxTollAmountPerDay,
            tollDurationMinutes,
            congestionTaxAmounts,
            tollFreeVehicle);
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (TollFreeVehicle.Contains(vehicle))
            return true;
        return false;
    }

    public bool IsTollFreeDate(DateTime dateTime)
    {
        if (TollFreeDate.Any(x => x <= dateTime && dateTime < x.AddDays(1)))
            return true;
        return false;
    }

    public int GetTollFeeForTime(TimeOnly time)
    {
        var amount = CongestionTaxAmounts.FirstOrDefault(x => x.StartTime <= time && time <= x.EndTime);

        return amount?.Amount ?? 0;
    }
}
