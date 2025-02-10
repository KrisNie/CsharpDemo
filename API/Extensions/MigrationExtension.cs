using API.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder application)
    {
        using var serviceScope = application.ApplicationServices.CreateScope();
        using var applicationContext =
            serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();
        applicationContext.Database.Migrate();
    }
}