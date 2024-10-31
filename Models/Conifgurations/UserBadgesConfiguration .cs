using DevGuide.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DevGuide.Models.Configrations
{
    public class UserBadgesConfiguration : IEntityTypeConfiguration<User_Badges>
    {
        public void Configure(EntityTypeBuilder<User_Badges> builder)
        {
            builder.HasKey(ub => ub.Id);

            // Ensure that User_Id is a foreign key of type string
            builder.HasOne(ub => ub.User)
                .WithMany(u => u.Badges)  // Assuming User has a collection of badges
                .HasForeignKey(ub => ub.User_Id)
                .OnDelete(DeleteBehavior.NoAction);  // Optional depending on your deletion policy

            // Badge relationship
            builder.HasOne(ub => ub.Badge)
                .WithMany(b => b.Badges)  // Assuming Badge has a collection of user badges
                .HasForeignKey(ub => ub.Badge_Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
