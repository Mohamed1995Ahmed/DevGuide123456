using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            //builder.HasKey(q => q.Quiz_Id);
            //builder.Property(q => q.QuizName)
            //    .IsRequired()
            //    .HasMaxLength(100);
            //builder.Property(q => q.Score)
            //    .IsRequired();
            //builder.Property(q => q.Result)
            //    .HasMaxLength(50);

            //builder.HasOne(q => q.User)
            //    .WithMany(u => u.Quizzes)
            //    .HasForeignKey(q => q.User_Id);

            builder.HasKey(q => q.Id);
            builder.Property(q => q.QuizName)
               .IsRequired()
               .HasMaxLength(70);

            builder.Property(q => q.Number_Of_Questions).IsRequired();
            builder.Property(q => q.Duration).IsRequired();

            builder.HasMany(q => q.Questions).WithOne(q => q.Quiz).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
