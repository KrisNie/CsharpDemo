using API.Handlers;
using API.Models;

namespace API.Navigations;

public static class WeatherNavigator
{
    public static void Navigate(WebApplication application)
    {
        application.MapGet(
            "/weathers",
            (IWeatherHandler weatherHandler) => weatherHandler.Read().Result);
        application.MapPost(
            "/weathers",
            (IWeatherHandler weatherHandler, Weather weather) =>
                weatherHandler.Create(weather).Result);
        application.MapPut(
            "/weathers",
            (IWeatherHandler weatherHandler) => weatherHandler.Read().Result);
        application.MapDelete(
            "/weathers",
            (IWeatherHandler weatherHandler) => weatherHandler.Read().Result);
    }
}