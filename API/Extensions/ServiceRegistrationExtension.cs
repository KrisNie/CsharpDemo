using System.Reflection;
using API.Context;
using API.Handlers;
using API.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class ServiceRegistrationExtension
{
    public static void RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers().AddNewtonsoftJson();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        AddAuthentication(services);
        AddSwaggerGen(services);
        AddDbContextPool(services, configuration);
        AddDependencies(services);
        AddCors(services);
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication();
        services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<IdentityContext>()
            .AddApiEndpoints();
        // services.AddIdentityApiEndpoints<IdentityUser>()
        //     .AddEntityFrameworkStores<IdentityContext>();
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
        services.AddDbContext<IdentityContext>(
            builder => builder.UseNpgsql(configuration.GetConnectionString("PostgresIdentity")));
        services.AddDbContextPool<WeatherContext>(
            builder => builder.UseNpgsql(configuration.GetConnectionString("PostgresDemo")));
    }

    // Dependency injection (DI) container
    private static void AddDependencies(IServiceCollection services)
    {
        services.AddScoped<IWeatherHandler, WeatherHandler>();
    }

    private static void AddSwaggerGen(IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Demo API",
                        Description = "A demo API",
                    });

                options.DocumentFilter<JsonPatchDocumentFilter>();
                // FIXME: using System.Reflection;
                options.IncludeXmlComments(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });
    }

    private static void AddCors(IServiceCollection services)
    {
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
}