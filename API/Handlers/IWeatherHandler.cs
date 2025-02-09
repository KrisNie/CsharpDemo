using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Handlers;

public interface IWeatherHandler
{
    Task<List<Weather>> Read();
    Task<IResult> Create(Weather weather);
    Task<IResult> Update(string city, DateOnly date, Weather weather);

    /// <summary>
    /// Updates, adds or removes records using JsonPatch.
    /// </summary>
    /// <param name="city"></param>
    /// <param name="date"></param>
    /// <param name="weatherPatch"></param>
    /// <returns></returns>
    Task<IResult> Patch(string city, DateOnly date, JsonPatchDocument<Weather>? weatherPatch);

    Task<IResult> Delete(string city, DateOnly date);
}