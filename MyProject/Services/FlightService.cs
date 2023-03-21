using Microsoft.EntityFrameworkCore;
using MyProject.Controllers;
using MyProject.Model;

namespace MyProject.Services;

public class FlightService
{
    private readonly FlightDbContext _dbContext;

    public FlightService(FlightDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FlightDto?> GetFlightAsync(string id, CancellationToken ct)
    {
        var flight = await _dbContext.Flights.Where(flight => flight.Id == id).SingleOrDefaultAsync(ct);
        if (flight is null)
        {
            return null;
        }

        var departureCity =
            await _dbContext.Cities.Where(city => city.Id == flight.DepartureCity).SingleOrDefaultAsync(ct);
        var arrivalCity =
            await _dbContext.Cities.Where(city => city.Id == flight.ArrivalCity).SingleOrDefaultAsync(ct);
        if (departureCity is null || arrivalCity is null)
        {
            return null;
        }

        return new FlightDto(flight.Id, departureCity.Name, arrivalCity.Name);
    }
}