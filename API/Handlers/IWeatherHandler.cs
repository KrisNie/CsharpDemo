using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Handlers;

public interface IWeatherHandler
{
    Task<List<Weather>> Read();
    Task<IResult> Create(Weather weather);
    Task<IResult> Update(string city, DateOnly date, Weather weather);
    Task<IResult> Patch(string city, DateOnly date, JsonPatchDocument<Weather>? weatherPatch);
    Task<IResult> Delete(string city, DateOnly date);
}