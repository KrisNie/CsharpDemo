using System.Text.Json;
using API.Context;
using API.Handlers;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ServiceRegistrationExtension
{
    public static void RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<JsonOptions>(
            options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
        AddDbContextPool(services, configuration);
        AddDependencies(services);
        services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
                            .WithExposedHeaders("X-Pagination");
                    });
            });
    }

    /// <summary>
    /// A typical unit-of-work when using Entity Framework Core (EF Core) involves:
    /// <list type="bullet">
    ///     <item> Creation of a DbContext instance </item>
    ///     <item>
    ///     Tracking of entity instances by the context. Entities become tracked by
    ///     <para>- Being returned from a query</para>
    ///     - Being added or attached to the context
    ///     </item>
    ///     <item>Changes are made to the tracked entities as needed to implement the business rule</item>
    ///     <item>SaveChanges or SaveChangesAsync is called. EF Core detects the changes made and writes them to the database.</item>
    ///     <item>The DbContext instance is disposed</item>
    /// </list>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    private static void AddDbContextPool(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<WeatherContext>(
            opt => opt.UseNpgsql(configuration.GetConnectionString("PostgresDemo")));
    }

    // Dependency injection (DI) container
    private static void AddDependencies(IServiceCollection services)
    {
        services.AddScoped<IWeatherHandler, WeatherHandler>();
    }
}