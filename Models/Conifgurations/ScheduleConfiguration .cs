using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            //builder.HasKey(s => new { s.User_Id, s.Date, s.StartTime });
            builder.HasKey(x => x.Id);
            builder.Property(s => s.Day).IsRequired();
            builder.Property(s => s.StartTime)
                .IsRequired();
            builder.Property(s => s.EndTime)
                .IsRequired();

            builder.HasOne(s => s.User)
                .WithMany(s => s.Schedules) // Assuming no navigation property in User
                .HasForeignKey(s => s.User_Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
