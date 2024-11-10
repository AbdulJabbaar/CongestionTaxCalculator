using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.Enums;
using CongestionTaxCalculator.Domain.City.Exceptions;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City;

public class City : AggregateRoot<CityId, int>
{
    public CityName Name { get; private set; }

    private List<CongestionTaxRulePerYear> _congestionTaxRulePerYears = [];
    public IReadOnlyList<CongestionTaxRulePerYear> CongestionTaxRulePerYears => _congestionTaxRulePerYears.AsReadOnly();

    private City() { }
    private City(CityId id, CityName name, List<CongestionTaxRulePerYear> congestionTaxRulePerYears) : base(id)
    {
        Name = name;
        _congestionTaxRulePerYears = congestionTaxRulePerYears;
    }

    public static City Create(CityId id, CityName name, List<CongestionTaxRulePerYear> congestionTaxRulePerYears)
    {
        return new City(id, name, congestionTaxRulePerYears);
    }

    public int GetTollFee(Vehicle vehicle, DateTime[] tollingStationsPassesDates)
    {
        if (tollingStationsPassesDates == null || tollingStationsPassesDates.Length == 0)
            return 0;

        var year = tollingStationsPassesDates[0].Year;
        var congestionTaxRuleYear = CongestionTaxRulePerYears.FirstOrDefault(x => x.Year == year)
            ?? throw new TaxRuleNotFoundException("Tax rules not found for the specified year");

        if (congestionTaxRuleYear.IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        var lstGroupedByDate = tollingStationsPassesDates.OrderBy(x => x).GroupBy(g => g.Date).ToDictionary(g => g.Key, g => g.ToList());

        int totalFee = 0;
        foreach (var groupedByDate in lstGroupedByDate)
        {
            int dailyFee = 0;
            var tollingDates = groupedByDate.Value
                .Where(date => !congestionTaxRuleYear.IsTollFreeDate(date))
                .OrderBy(date => date)
                .ToList();

            if (tollingDates.Count == 0)
            {
                continue;
            }

            var benchmarkDate = tollingDates[0];
            for (int i = 0; i < tollingDates.Count; i++)
            {               
                var currentTax = congestionTaxRuleYear.GetTollFeeForTime(TimeOnly.FromDateTime(tollingDates[i]));
                dailyFee += currentTax;

                // if the next pass is with 1 hour of the benchmark
                if (i + 1 < tollingDates.Count)
                {
                    if ((tollingDates[i + 1] - benchmarkDate).TotalHours <= 1)
                    {
                        var (skipDates, nextTax) = ManipulateTheHourRule(tollingDates, benchmarkDate, congestionTaxRuleYear);
                        // adding the difference if nextTax is grater
                        if (nextTax > currentTax)
                        {
                            dailyFee += nextTax - currentTax;
                        }
                        i = i + skipDates;
                        benchmarkDate = tollingDates[i];
                    }
                    else
                    {
                        benchmarkDate = tollingDates[i + 1];
                    }
                }
            }
            dailyFee = Math.Min(dailyFee, 60);
            totalFee += dailyFee;
        }

        return totalFee;
    }

    private (int, int) ManipulateTheHourRule(List<DateTime> dates, DateTime currentStartTime, CongestionTaxRulePerYear congestionTaxRuleYear)
    {
        var datesInAnHour = dates.Where(date => date >= currentStartTime && date < currentStartTime.AddMinutes(congestionTaxRuleYear.TollDurationMinutes)).ToList();
        var amount = 0;
        foreach (var item in datesInAnHour)
        {
            var tollAmount = congestionTaxRuleYear.GetTollFeeForTime(TimeOnly.FromDateTime(item));
            if (tollAmount > amount)
            {
                amount = tollAmount;
            }
        }
        return (datesInAnHour.Count - 1, amount);
    }
}
