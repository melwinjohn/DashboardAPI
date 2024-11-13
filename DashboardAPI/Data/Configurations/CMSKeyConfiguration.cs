using DashboardAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Data.Configurations
{
    public class CMSKeyConfiguration : IEntityTypeConfiguration<CMSKey>
    {
        public void Configure(EntityTypeBuilder<CMSKey> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(x => x.CMSKeyValues)
                .WithOne(x => x.CMSKey)
                .HasForeignKey(x => x.KeyID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
