using DashboardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Data
{
    public class MultilingualDbContext : DbContext
    {
        public MultilingualDbContext(DbContextOptions<MultilingualDbContext> options)
            : base(options)
        {
        }

        public DbSet<LanguageMaster> LanguageMaster { get; set; }
        public DbSet<CMSKey> CMSKey { get; set; }
        public DbSet<CMSKeyValue> CMSKeyValue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MultilingualDbContext).Assembly);
        }
    }
}
