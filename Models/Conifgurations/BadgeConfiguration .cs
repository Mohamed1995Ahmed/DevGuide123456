using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

namespace DevGuide.Models.Configrations
{
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.BadgeType)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(b => b.Badges)
                .WithOne(ub => ub.Badge)
                .HasForeignKey(ub => ub.Badge_Id).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
