using API.Context;
using API.Models;
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
        return Results.Created($"/weather/{weather.City}", weather);
    }

    public Task<IResult> Update(Weather weather)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> Delete(Weather weather)
    {
        throw new NotImplementedException();
    }
}