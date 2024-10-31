using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

namespace DevGuide.Models.Configrations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            //builder.HasKey(qq => new { qq.Quiz_Id, qq.Questions });
            //builder.Property(qq => qq.Questions)
            //    .IsRequired();

            //builder.HasOne(qq => qq.Quiz)
            //    .WithMany(q => q.Questions)
            //    .HasForeignKey(qq => qq.Quiz_Id);

            builder.HasKey(q => q.Id);
            builder.Property(q => q.Text).IsRequired().HasMaxLength(1000);

            builder.HasOne(q => q.Quiz)
                .WithMany(q => q.Questions).
                HasForeignKey(q => q.Quiz_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(q => q.Options)
                .WithOne(q => q.Question)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(q=>q.Skill)
                .WithMany(q=>q.Questions)
                .HasForeignKey(q => q.Skill_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(q=>q.User_Answers)
                .WithOne(q=>q.Question)
                .OnDelete(DeleteBehavior.NoAction);






        }
    }
}
