using API.Resource;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetingController(ILogger<WeatherForecastController> logger)
{
    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "greeting")]
    public string Greeting()
    {
        return GreetingResources.Greeting;
    }
}