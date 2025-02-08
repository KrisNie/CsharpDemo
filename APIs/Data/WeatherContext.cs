using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Models;

namespace MinimalAPIs.Data;

public class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    public DbSet<Weather> Weathers { get; set; } = null!;
}