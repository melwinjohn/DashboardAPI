using DashboardAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Data.Configurations
{
    public class LanguageMasterConfiguration : IEntityTypeConfiguration<LanguageMaster>
    {
        public void Configure(EntityTypeBuilder<LanguageMaster> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(x => x.CMSKeyValues)
                .WithOne(x => x.Language)
                .HasForeignKey(x => x.LangID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
