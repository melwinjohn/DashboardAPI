using DashboardAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Data.Configurations
{
    public class CMSKeyValueConfiguration : IEntityTypeConfiguration<CMSKeyValue>
    {
        public void Configure(EntityTypeBuilder<CMSKeyValue> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(500);

            // Relationships are defined in the parent entity configurations
        }
    }
}
