namespace MinimalAPIs.Models;

public class Weather
{
    public required string City { get; set; }
    public int LowTemperature { get; set; }
    public int HighTemperature { get; set; }
    public float Precipitation { get; set; }
    public required DateOnly Date { get; set; }

    /// <summary>
    /// Wind speed in kmph
    /// </summary>
    public float WindSpeed { get; set; }
}