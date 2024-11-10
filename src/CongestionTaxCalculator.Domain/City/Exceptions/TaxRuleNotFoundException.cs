namespace CongestionTaxCalculator.Domain.City.Exceptions;

public class TaxRuleNotFoundException : Exception
{
    public TaxRuleNotFoundException(string message) : base(message) { }
}
