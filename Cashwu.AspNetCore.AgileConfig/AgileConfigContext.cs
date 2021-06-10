using Microsoft.EntityFrameworkCore;

namespace Cashwu.AspNetCore.AgileConfig
{
    public class AgileConfigContext : DbContext
    {
        public AgileConfigContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<AgileConfigValue> ConfigValue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AgileConfigValue>(t =>
            {
                t.ToTable("AgileConfigValue");

                t.HasKey(x => x.Key);

                t.Property(x => x.Key)
                 .HasMaxLength(64);

                t.Property(x => x.LastUpdatedOn)
                 .HasDefaultValueSql("GETUTCDATE()")
                 .HasField("_lastUpdated");

                t.HasIndex(x => x.LastUpdatedOn);
            });
        }
    }
}