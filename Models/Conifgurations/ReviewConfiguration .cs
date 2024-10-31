using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;
using System.Reflection.Emit;


namespace DevGuide.Models.Configrations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Description)
                .HasMaxLength(int.MaxValue);

            builder.Property(r => r.Rate).IsRequired();

            builder.Property(r => r.ReviewDate);

            builder.Property(r => r.Description).IsRequired().HasMaxLength(int.MaxValue);
           



            builder.HasOne(r => r.Session)
        .WithOne(s => s.Review)
        .HasForeignKey<Review>(r => r.Session_Id)
        .OnDelete(DeleteBehavior.NoAction); // Disable cascading deletes

            builder.HasOne(u => u.User)
                .WithMany(r => r.Reviews).
                HasForeignKey(r=>r.UserId)
                .OnDelete(DeleteBehavior.NoAction);






            //builder.HasOne(r => r.User)
            //    .WithMany(u => u.Reviews)
            //    .HasForeignKey(r => r.User_Id)
            //     .OnDelete(DeleteBehavior.NoAction); // Disable cascade delete

            //builder.HasOne(r => r.Session)
            //    .WithOne(s => s.Review)  // One-to-one relationship
            //    .HasForeignKey<Review>(r => r.Session_Id);

        }
    }
}
