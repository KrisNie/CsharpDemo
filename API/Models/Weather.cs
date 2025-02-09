using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Table("weather")]
[PrimaryKey(nameof(City), nameof(Date))]
public class Weather
{
    [Column("city")]
    [JsonPropertyName("City")]
    [MaxLength(80)]
    public required string City { get; set; }

    [Column("date")] public required DateOnly Date { get; set; }
    [Column("temperature_low")] public int LowTemperature { get; set; }
    [Column("temperature_high")] public int HighTemperature { get; set; }
    [Column("precipitation")] public float Precipitation { get; set; }

    /// <summary>
    /// Wind speed in kmph
    /// </summary>
    [Column("wind_speed_kmph")]
    [JsonPropertyName("WindSpeed")]
    public float WindSpeed { get; set; }
}