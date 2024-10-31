using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

//public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
//{
//    public void Configure(EntityTypeBuilder<Payment> builder)
//    {
//        builder.HasKey(p => p.Payment_Id);
//        builder.Property(p => p.PaymentType)
//            .IsRequired()
//            .HasMaxLength(100);
//        builder.Property(p => p.Amount)
//            .IsRequired();
//        builder.Property(p => p.Tax)
//            .IsRequired();
//        builder.Property(p => p.Total)
//            .IsRequired();

//        // Configure the one-to-one relationship with Session
//        builder.HasOne(p => p.Session)
//            .WithOne(s => s.Payment)
//            .HasForeignKey<Payment>(p => p.Session_Id);

//        // Configure the relationship with User
//        builder.HasOne(p => p.User)
//            .WithMany(u => u.Payments)  // Assuming User has ICollection<Payment>
//            .HasForeignKey(p => p.User_Id)
//            .OnDelete(DeleteBehavior.NoAction);  // Specify NO ACTION on delete
//    }
//}

namespace DevGuide.Models.Configrations
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Text).IsRequired().HasMaxLength(100);
            builder.Property(c => c.IsCorrect).IsRequired();

            builder.HasOne(x => x.Question)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.Question_Id).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(o=>o.UserAnswer)
                .WithOne(o=>o.Option)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o=>o.Skill).
                WithMany(x => x.Options).
                HasForeignKey(o=>o.Skill_Id).
                OnDelete(DeleteBehavior.NoAction);


        }
    }
}


