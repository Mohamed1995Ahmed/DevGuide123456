using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

//public class QuizAnswersConfiguration : IEntityTypeConfiguration<QueryAnswer>
//{
//    public void Configure(EntityTypeBuilder<QueryAnswer> builder)
//    {
//        builder.HasKey(qa => new { qa.Quiz_Id, qa.Answers });
//        builder.Property(qa => qa.Answers)
//            .IsRequired();

//        builder.HasOne(qa => qa.Quiz)
//            .WithMany(q => q.Answers)
//            .HasForeignKey(qa => qa.Quiz_Id);
//    }
//}

namespace DevGuide.Models.Configrations
{
    public class QueryAnswerConfiguration : IEntityTypeConfiguration<QueryAnswer>
    {
        public void Configure(EntityTypeBuilder<QueryAnswer> builder)
        {
            builder.HasKey(qa => qa.Id);
           
            builder.Property(b => b.Id).ValueGeneratedOnAdd();


            builder.Property(qa => qa.Answer)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(qa => qa.File)
                .HasMaxLength(int.MaxValue);

            builder.Property(s => s.DateTime).IsRequired();
            builder.HasOne(qa => qa.Query)
                .WithMany(q => q.QueryAnswers)
                .HasForeignKey(qa => qa.Query_Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
