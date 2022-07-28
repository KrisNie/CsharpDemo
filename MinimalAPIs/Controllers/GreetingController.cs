using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIs.Resource;

namespace MinimalAPIs.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetingController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public GreetingController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Greeting")]
    public string Greeting()
    {
        return GreetingResources.Greeting;
    }
}