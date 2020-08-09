using Microsoft.EntityFrameworkCore;
using MvcDemo.Data.Girl;

namespace MvcDemo.Data.Base
{
    public class LifeContext : DbContext
    {
        public LifeContext(DbContextOptions<LifeContext> options) : base(options)
        {
        }

        public DbSet<GirlEntity> Girls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GirlEntity>().ToTable("girl");
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLazyLoadingProxies();
        }
    }
}