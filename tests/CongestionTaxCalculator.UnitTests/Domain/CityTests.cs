using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.Entities;
using CongestionTaxCalculator.Domain.City.Enums;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using FluentAssertions;
using PublicHoliday;

namespace CongestionTaxCalculator.UnitTests.Domain;

public class CityTests
{
    [Fact]
    public void GetTollFee_Should_ReturnValidTax_ForDifferentDays()
    {
        // Arrange
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
        var city = City.Create(CityId.Create(10), CityName.Stockholm, [congetionTaxRules]);

        List<DateTime> validTollDays = [
            new DateTime(2013, 5, 15, 6, 5, 0),
            new DateTime(2013, 5, 15, 6, 45, 0), // 13
            new DateTime(2013, 6, 16, 6, 20, 0), // Toll Free day
            new DateTime(2013, 7, 17, 7, 5, 0), // Toll Free day
            new DateTime(2013, 8, 15, 7, 5, 0),
            new DateTime(2013, 8, 15, 7, 45, 0), //18
            new DateTime(2013, 8, 18, 7, 45, 0), // Toll Free day
            new DateTime(2013, 8, 19, 7, 45, 0), //18
            new DateTime(2013, 8, 20, 7, 45, 0), //18
            new DateTime(2013, 8, 21, 7, 45, 0), //18
            ];

        // Act
        var tollFee = city.GetTollFee(Vehicle.Create(VehicleType.Regular), validTollDays.ToArray());

        // Assert
        tollFee.Should().Be(85);
    }

    [Fact]
    public void GetTollFee_Should_ReturnValidTax_ForOneDay_WithTaxLessThan_60()
    {
        // Arrange
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
        var city = City.Create(CityId.Create(10), CityName.Stockholm, [congetionTaxRules]);

        List<DateTime> validTollDays = [
            new DateTime(2013, 5, 15, 8, 35, 0), // (8)
            new DateTime(2013, 5, 15, 11, 30, 0), // (8)
            new DateTime(2013, 5, 15, 13, 42, 0), // (8)
            new DateTime(2013, 5, 15, 14, 45, 0), // Same Hour - 1 
            new DateTime(2013, 5, 15, 14, 55, 0), // Same Hour - 1 
            new DateTime(2013, 5, 15, 14, 59, 0), // Same Hour - 1 = (8)
            ];

        // Act
        var tollFee = city.GetTollFee(Vehicle.Create(VehicleType.Regular), validTollDays.ToArray());

        // Assert
        tollFee.Should().Be(32);
    }

    [Fact]
    public void GetTollFee_Should_ReturnValidTax_ForOneDay()
    {
        // Arrange
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
        var city = City.Create(CityId.Create(10), CityName.Stockholm, [congetionTaxRules]);

        List<DateTime> validTollDays = [
            new DateTime(2013, 5, 15, 6, 5, 0),
            new DateTime(2013, 5, 15, 6, 45, 0), // 13
            new DateTime(2013, 5, 15, 6, 20, 0),
            new DateTime(2013, 5, 15, 7, 5, 0),
            new DateTime(2013, 5, 15, 7, 45, 0), // 18
            new DateTime(2013, 5, 15, 8, 45, 0), // 8
            new DateTime(2013, 5, 15, 15, 0, 0),
            new DateTime(2013, 5, 15, 15, 30, 0),// 18
            new DateTime(2013, 5, 15, 17, 45, 0), // 13
            new DateTime(2013, 5, 15, 18, 25, 0), // 8
            new DateTime(2013, 5, 15, 18, 45, 0),
            new DateTime(2013, 5, 15, 18, 50, 0),
            new DateTime(2013, 5, 15, 18, 51, 0),
            ];

        // Act
        var tollFee = city.GetTollFee(Vehicle.Create(VehicleType.Regular), validTollDays.ToArray());

        // Assert
        tollFee.Should().Be(60);
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
