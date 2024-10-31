using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(us => us.Id).ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.HasMany(s => s.UserSkills)
                .WithOne(u => u.Skill)
                .HasForeignKey(s => s.Skill_Id).
                OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s=>s.Options).WithOne(o => o.Skill).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Questions).WithOne(o => o.Skill).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
