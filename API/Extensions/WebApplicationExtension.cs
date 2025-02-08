using API.Navigations;

namespace API.Extensions;

public static class WebApplicationExtension
{
    public static void EndpointsMap(this WebApplication application)
    {
        WeatherNavigator.Navigate(application);
    }
}