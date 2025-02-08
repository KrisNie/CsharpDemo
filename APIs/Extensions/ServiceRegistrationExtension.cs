using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Data;

namespace MinimalAPIs.Extensions;

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
        AddDbContextPool(services, configuration);
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

    private static void AddDbContextPool(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<WeatherContext>(
            opt => opt.UseNpgsql(configuration.GetConnectionString("PostgresDemo")));
    }

    // TODO: private method for Dependency injection (DI) container
}