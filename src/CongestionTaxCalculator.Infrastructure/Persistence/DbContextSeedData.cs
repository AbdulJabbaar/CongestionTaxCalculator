using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.Enums;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using Microsoft.EntityFrameworkCore;
using PublicHoliday;

namespace CongestionTaxCalculator.Infrastructure.Persistence;

public class DbContextSeedData
{
    private readonly InMemoryDbContext _context;

    public DbContextSeedData(InMemoryDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var cities = await _context.Cities.CountAsync();
        if (cities == 0)
        {
            await _context.Cities.AddAsync(GetCity());
            await _context.SaveChangesAsync();
        }
    }

    private City GetCity()
    {
        var holidays = GetPublicHolidays(2013);
        var dayBeforeHolidays = holidays.Select(x => x.AddDays(-1)).ToArray();
        var weekends = GetWeekendDates(2013);
        var monthOfJuly = GetDates(2013, 7);
        DateTime[] taxFreeDays = [.. holidays, .. dayBeforeHolidays, .. weekends, .. monthOfJuly];

        var congetionTaxRules = CongestionTaxRulePerYear.Create(
            TaxRulesId.Create(Guid.NewGuid()),
            2013,
            taxFreeDays,
            60,
            60,
            CongestionTaxAmounts,
            TollFreeVehicle);
        return City.Create(CityId.Create(10), CityName.Gothenburg, [congetionTaxRules]);
    }

    private List<DateTime> GetWeekendDates(int year)
    {
        List<DateTime> weekendList = [];
        for (DateTime date = new DateTime(year, 1, 1); date <= new DateTime(year, 12, 31); date = date.AddDays(1))
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                weekendList.Add(date);
        }

        return weekendList;
    }

    public List<DateTime> GetDates(int year, int month)
    {
        var dates = new List<DateTime>();

        for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
        {
            dates.Add(date);
        }

        return dates;
    }

    private List<DateTime> GetPublicHolidays(int year) => new SwedenPublicHoliday().PublicHolidays(year).ToList();

    private List<CongestionTaxAmount> CongestionTaxAmounts => [
            CongestionTaxAmount.Create(new TimeOnly(6,0,0), new TimeOnly(6,29,59), 8),
            CongestionTaxAmount.Create(new TimeOnly(6,30,0), new TimeOnly(6,59,59), 13),
            CongestionTaxAmount.Create(new TimeOnly(7,0,0), new TimeOnly(7,59,59), 18),
            CongestionTaxAmount.Create(new TimeOnly(8,0,0), new TimeOnly(8,29,59), 13),
            CongestionTaxAmount.Create(new TimeOnly(8,30,0), new TimeOnly(14,59,59), 8),
            CongestionTaxAmount.Create(new TimeOnly(15,0,0), new TimeOnly(15,29,59), 13),
            CongestionTaxAmount.Create(new TimeOnly(15,30,0), new TimeOnly(16,59,59), 18),
            CongestionTaxAmount.Create(new TimeOnly(17,0,0), new TimeOnly(17,59,59), 13),
            CongestionTaxAmount.Create(new TimeOnly(18,0,0), new TimeOnly(18,29,59), 8),
            CongestionTaxAmount.Create(new TimeOnly(18,30,0), new TimeOnly(05,59,59), 0),
            ];
    private List<Vehicle> TollFreeVehicle => [
            Vehicle.Create(VehicleType.Emergency),
            Vehicle.Create(VehicleType.Diplomat),
            Vehicle.Create(VehicleType.Motorcycle),
            Vehicle.Create(VehicleType.Military),
            Vehicle.Create(VehicleType.Foreign)
            ];
}
