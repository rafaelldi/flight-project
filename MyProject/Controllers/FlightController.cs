using Microsoft.AspNetCore.Mvc;
using MyProject.Exceptions;
using MyProject.Services;

namespace MyProject.Controllers;

[ApiController]
[Route("flights")]
public class FlightController : ControllerBase
{
    private readonly FlightService _flightService;

    public FlightController(FlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet("{id}")]
    public async Task<FlightDto> GetFlight(string id, CancellationToken ct)
    {
        var flight = await _flightService.GetFlightAsync(id, ct);
        if (flight is null)
        {
            throw new NotFoundException("Unable to find the flight");
        }

        return flight;
    }
}