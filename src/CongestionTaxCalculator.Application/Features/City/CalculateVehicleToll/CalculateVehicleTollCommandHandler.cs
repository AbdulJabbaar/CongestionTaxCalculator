using CongestionTaxCalculator.Application.Common.Exceptions;
using CongestionTaxCalculator.Application.Repositories;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.City.CalculateVehicleToll;

public class CalculateVehicleTollCommandHandler(ICityRepository cityRepository) : IRequestHandler<CalculateVehicleTollCommand, int>
{
    public async Task<int> Handle(CalculateVehicleTollCommand request, CancellationToken cancellationToken)
    {
        var city = await cityRepository.GetCityAsync(request.CityName, cancellationToken);
        if (city is null)
        {
            throw new CityNotFoundException("City not found");
        }

        var tollAmount = city.GetTollFee(Vehicle.Create(request.Vehicle), request.DatePassesToll);

        return tollAmount;
    }
}
