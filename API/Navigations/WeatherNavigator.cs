using API.Handlers;
using API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Navigations;

public static class WeatherNavigator
{
    public static void Navigate(WebApplication application)
    {
        var group = application.MapGroup("weathers").WithTags("Weathers");
        group.MapGet("", Read);
        group.MapPost("", Create);
        group.MapPut("/{city}/{date}", Update);
        group.MapPatch("/{city}/{date}", Patch);
        group.MapDelete("/{city}/{date}", Delete);
    }

    private static List<Weather> Read(IWeatherHandler weatherHandler)
    {
        return weatherHandler.Read().Result;
    }

    private static IResult Create(IWeatherHandler weatherHandler, [FromBody] Weather weather)
    {
        return weatherHandler.Create(weather).Result;
    }

    [Produces("application/json")]
    private static IResult Update(
        IWeatherHandler weatherHandler,
        string city,
        DateOnly date,
        [FromBody] Weather weather)
    {
        return weatherHandler.Update(city, date, weather).Result;
    }

    /// <summary>
    /// Patch
    /// </summary>
    /// <param name="weatherHandler"></param>
    /// <param name="city"></param>
    /// <param name="date"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Produces("application/json-patch+json")]
    [ProducesResponseType<Weather>(statusCode: 200)]
    private static async Task<IResult> Patch(
        IWeatherHandler weatherHandler,
        string city,
        DateOnly date,
        HttpRequest request)
    {
        var weatherPatch =
            JsonConvert.DeserializeObject<JsonPatchDocument<Weather>>(
                await new StreamReader(request.Body).ReadToEndAsync());
        return await weatherHandler.Patch(
            city,
            date,
            weatherPatch);
    }

    private static IResult Delete(IWeatherHandler weatherHandler, string city, DateOnly date)
    {
        return weatherHandler.Delete(city, date).Result;
    }
}