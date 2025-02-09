using API.Context;
using API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace API.Handlers;

public class WeatherHandler(WeatherContext weatherContext) : IWeatherHandler
{
    public async Task<List<Weather>> Read()
    {
        return await weatherContext.Weathers.ToListAsync();
    }

    public async Task<IResult> Create(Weather weather)
    {
        if (weatherContext.Weathers.Any(x => x.City == weather.City && x.Date == weather.Date))
            return Results.Conflict();
        await weatherContext.Weathers.AddAsync(weather);
        await weatherContext.SaveChangesAsync();
        return Results.Created($"/weather/{weather.City}/{weather.Date}", weather);
    }

    public async Task<IResult> Update(string city, DateOnly date, Weather weather)
    {
        var targetWeather = await weatherContext.Weathers.FindAsync(city, date);
        if (targetWeather is null) return Results.NotFound();
        weatherContext.Entry(targetWeather).CurrentValues.SetValues(weather);
        await weatherContext.SaveChangesAsync();
        return Results.Ok(weather);
    }

    public async Task<IResult> Patch(
        string city,
        DateOnly date,
        JsonPatchDocument<Weather>? weatherPatch)
    {
        if (weatherPatch == null) return Results.NoContent();
        var targetWeather = await weatherContext.Weathers.FindAsync(city, date);
        if (targetWeather is null) return Results.NotFound();
        weatherPatch.ApplyTo(targetWeather);
        weatherContext.Update(targetWeather);
        await weatherContext.SaveChangesAsync();
        return Results.Ok(targetWeather);
    }

    public async Task<IResult> Delete(string city, DateOnly date)
    {
        var targetWeather = await weatherContext.Weathers.FindAsync(city, date);
        if (targetWeather is null) return Results.NotFound();
        weatherContext.Weathers.Remove(targetWeather);
        await weatherContext.SaveChangesAsync();
        return Results.Ok();
    }
}