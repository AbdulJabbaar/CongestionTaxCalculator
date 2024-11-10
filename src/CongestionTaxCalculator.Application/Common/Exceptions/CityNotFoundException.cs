namespace CongestionTaxCalculator.Application.Common.Exceptions;

public class CityNotFoundException:Exception
{
    public CityNotFoundException(string message): base(message)
    {
        
    }
}
