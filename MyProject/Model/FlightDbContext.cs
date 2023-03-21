using Microsoft.EntityFrameworkCore;

namespace MyProject.Model;

public class FlightDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public DbSet<Flight> Flights { get; set; }
    public DbSet<City> Cities { get; set; }
    

    public FlightDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("Main"));
    }
}