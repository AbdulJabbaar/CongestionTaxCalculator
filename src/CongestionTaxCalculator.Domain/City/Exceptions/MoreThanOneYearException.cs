namespace CongestionTaxCalculator.Domain.City.Exceptions;

public class MoreThanOneYearException : Exception
{
    public MoreThanOneYearException(string message) : base(message) { }
}
