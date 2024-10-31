using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

namespace DevGuide.Models.Configrations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Topic)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(255);


            builder.Property(s => s.DateTime).IsRequired();

            builder.Property(s => s.Status);

            builder.Property(s => s.Duration).IsRequired();


            builder.Property(s => s.MeetingLink);


            // Configure the relationship to the User who is the regular user
            builder.HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.NoAction); // Cascade delete

            // Configure the relationship to the User who is the instructor
            builder.HasOne(s => s.User_Instructor)
                .WithMany(s => s.InstructedSessions) // Assuming no navigation property in User
                .HasForeignKey(s => s.User_Instructor_Id)
                .OnDelete(DeleteBehavior.NoAction); //

            builder.HasOne(s => s.Payment)
                .WithOne(s => s.Session).
                OnDelete(DeleteBehavior.NoAction);
            //    .HasForeignKey<Payment>(s => s.Session_Id)
            //    

            builder.HasOne(s => s.Review)
                .WithOne(s => s.Session)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
