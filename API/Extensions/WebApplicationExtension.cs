using API.Navigations;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static class WebApplicationExtension
{
    public static void EndpointsMap(this WebApplication application)
    {
        WeatherNavigator.Navigate(application);
        MapIdentityApi(application);
        application.MapControllers();
    }

    private static void MapIdentityApi(WebApplication application)
    {
        application.MapGroup("identity").WithTags("Identity").MapIdentityApi<IdentityUser>();
    }
}