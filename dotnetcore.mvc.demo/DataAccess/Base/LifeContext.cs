using dotnetcore.mvc.demo.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore.mvc.demo.DataAccess.Base
{
    public class LifeContext : DbContext
    {
        public LifeContext(DbContextOptions<LifeContext> options) : base(options)
        {
        }

        public DbSet<Girl> Girls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Girl>().ToTable("girl");
        }
    }
}