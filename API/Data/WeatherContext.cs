using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    public DbSet<Weather> Weathers { get; set; } = null!;
}