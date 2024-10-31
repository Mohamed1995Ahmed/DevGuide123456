using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{


    public class UserSkillsConfiguration : IEntityTypeConfiguration<User_Skills>
    {
        public void Configure(EntityTypeBuilder<User_Skills> builder)
        {
            builder.HasKey(us => us.Id);
            builder.Property(us=>us.Id).ValueGeneratedOnAdd();
            builder.HasOne(us => us.User)
                .WithMany(x => x.Skills) // Assuming no navigation property in User
                .HasForeignKey(us => us.User_Id).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(us => us.Skill)
             .WithMany(x => x.UserSkills) // Assuming no navigation property in User
             .HasForeignKey(us => us.Skill_Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
