using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetingController(ILogger<WeatherForecastController> logger)
{
    private readonly ILogger<WeatherForecastController> _logger = logger;

    /// <summary>
    /// Greetings to you.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "greeting")]
    [Produces("application/json")]
    public string Greeting()
    {
        return "Hello World!";
    }
}