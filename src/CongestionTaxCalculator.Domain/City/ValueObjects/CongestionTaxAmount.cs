using CongestionTaxCalculator.Domain.Common.Models;

namespace CongestionTaxCalculator.Domain.City.ValueObjects;

public class CongestionTaxAmount : ValueObject
{
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public int Amount { get; private set; }

    private CongestionTaxAmount(TimeOnly startTime, TimeOnly endTime, int amount)
    {
        StartTime = startTime;
        EndTime = endTime;
        Amount = amount;
    }

    public static CongestionTaxAmount Create(TimeOnly startTime, TimeOnly endTime, int amount)
    {
        return new CongestionTaxAmount(startTime, endTime, amount);
    }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
        yield return Amount;
    }

    private CongestionTaxAmount() { }
}
