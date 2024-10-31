using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

//public class QueryAnswerConfiguration : IEntityTypeConfiguration<QueryAnswer>
//{
//    public void Configure(EntityTypeBuilder<QueryAnswer> builder)
//    {
//        builder.HasKey(qa => qa.Answer_Id);
//        builder.Property(qa => qa.Answer)
//            .IsRequired()
//            .HasMaxLength(1000);
//        builder.Property(qa => qa.File)
//            .HasMaxLength(255);

//        builder.HasOne(qa => qa.Query)
//            .WithMany(q => q.QueryAnswers)
//            .HasForeignKey(qa => qa.Query_Id);
//    }
//}

namespace DevGuide.Models.Configrations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PaymentType)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Amount)
                .IsRequired();
            builder.Property(p => p.Tax)
                .IsRequired();
            builder.Property(p => p.Total)
                .IsRequired();

            // Configure the one-to-one relationship with Session
            builder.HasOne(p => p.Session)
            .WithOne(s => s.Payment)
            .HasForeignKey<Payment>(p => p.Session_Id) // Keep this
            .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
