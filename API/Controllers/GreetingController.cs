using API.Resource;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetingController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public GreetingController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "greeting")]
    public string Greeting()
    {
        return GreetingResources.Greeting;
    }
}