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
        application.MapGet("/weathers", Read);
        application.MapPost("/weathers", Create);
        application.MapPut("/weathers/{city}/{date}", Update);
        application.MapPatch("/weathers/{city}/{date}", Patch);
        application.MapDelete("/weathers/{city}/{date}", Delete);
    }

    private static List<Weather> Read(IWeatherHandler weatherHandler)
    {
        return weatherHandler.Read().Result;
    }

    private static IResult Create(IWeatherHandler weatherHandler, [FromBody] Weather weather)
    {
        return weatherHandler.Create(weather).Result;
    }

    private static IResult Update(
        IWeatherHandler weatherHandler,
        string city,
        DateOnly date,
        [FromBody] Weather weather)
    {
        return weatherHandler.Update(city, date, weather).Result;
    }

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