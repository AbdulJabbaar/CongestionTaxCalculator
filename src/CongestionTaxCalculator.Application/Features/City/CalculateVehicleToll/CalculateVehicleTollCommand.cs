using CongestionTaxCalculator.Domain.City.Enums;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.City.CalculateVehicleToll;

public record CalculateVehicleTollCommand(CityName CityName, VehicleType Vehicle, DateTime[] DatePassesToll): IRequest<int>;
