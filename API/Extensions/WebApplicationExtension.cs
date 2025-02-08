using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class WebApplicationExtension
{
    public static void EndpointsMap(this WebApplication application)
    {
        application.MapGet("/weathers", async (WeatherContext c) => await c.Weathers.ToListAsync());
    }
}