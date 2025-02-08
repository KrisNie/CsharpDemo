using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("weather")]
[PrimaryKey(nameof(City), nameof(Date))]
public class Weather
{
    [Column("city")] public required string City { get; set; }
    [Column("temperature_low")] public int LowTemperature { get; set; }
    [Column("temperature_high")] public int HighTemperature { get; set; }
    [Column("precipitation")] public float Precipitation { get; set; }
    [Column("date")] public required DateOnly Date { get; set; }

    /// <summary>
    /// Wind speed in kmph
    /// </summary>
    [Column("wind_speed_kmph")]
    public float WindSpeed { get; set; }
}