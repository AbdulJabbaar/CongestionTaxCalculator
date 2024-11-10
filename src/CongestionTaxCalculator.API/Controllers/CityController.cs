using CongestionTaxCalculator.API.Requests;
using CongestionTaxCalculator.API.Response;
using CongestionTaxCalculator.Application.Features.City.CalculateVehicleToll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CityController : ControllerBase
{        
    private readonly ILogger<CityController> _logger;
    private readonly ISender _sender;

    public CityController(ILogger<CityController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CalculateVehicleTax([FromForm] CalculateVehicleTaxRequest request, CancellationToken cancellationToken)
    {
        var command = new CalculateVehicleTollCommand(request.CityName, request.VehicleType, request.DatePassesToll);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(new VehicleTaxResponse(result));
    }
}
