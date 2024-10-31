using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{
    public class SupportConfiguration : IEntityTypeConfiguration<Support>
    {
        public void Configure(EntityTypeBuilder<Support> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.PhoneNumber)
                 .IsRequired()
                .HasMaxLength(15);

            builder.Property(s => s.ObjectOfComplain)
                 .IsRequired()
                .HasMaxLength(500);


            builder.Property(s => s.Message)
                 .IsRequired()
                .HasMaxLength(500);


            builder.Property(s => s.ObjectOfComplain)
                 .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Date)
                .IsRequired();



            builder.HasOne(s => s.User)
                .WithMany(u => u.Supports)
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
