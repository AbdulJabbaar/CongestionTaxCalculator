using FluentValidation;

namespace CongestionTaxCalculator.Application.Features.City.CalculateVehicleToll;

public class CalculateVehicleTollCommandValidator : AbstractValidator<CalculateVehicleTollCommand>
{
    public CalculateVehicleTollCommandValidator()
    {
        RuleFor(x => x.Vehicle).IsInEnum();
        RuleFor(x => x.CityName).IsInEnum();
    }
}
