using API.Models;

namespace API.Handlers;

public interface IWeatherHandler
{
    Task<List<Weather>> Read();
    Task<IResult> Create(Weather weather);
    Task<IResult> Update(Weather weather);
    Task<IResult> Delete(Weather weather);
}