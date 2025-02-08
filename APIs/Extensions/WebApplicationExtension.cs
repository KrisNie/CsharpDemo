using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Data;

namespace MinimalAPIs.Extensions;

public static class WebApplicationExtension
{
    public static void EndpointsMap(this WebApplication application)
    {
        application.MapGet("/pizzas", async (WeatherContext c) => await c.Weathers.ToListAsync());
    }
}