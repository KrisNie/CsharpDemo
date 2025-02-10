using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public class IdentityContext(DbContextOptions<IdentityContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<IdentityUser>(entity => { entity.ToTable(name: "User"); });
        builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Role"); });
        // In case you changed the TKey type
        // entity.HasKey(key => new { key.UserId, key.RoleId });
        builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
        builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
        // In case you changed the TKey type
        //  entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });  
        builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
        builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
        // In case you changed the TKey type
        // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
        builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

        builder.HasDefaultSchema("identity");
    }
}