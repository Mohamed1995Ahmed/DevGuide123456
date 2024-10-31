using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{


    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name (if not default)
            builder.ToTable("Users");

            // Primary key (IdentityUser already has Id as key)
            builder.HasKey(u => u.Id);

            // Property configurations
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);  // Set max length for the Name field

            // Property configurations
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);  // Set max length for the Name field

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.UserName).IsUnique();

            builder.Property(u => u.CV)
                .HasMaxLength(int.MaxValue); // Optional, allow max length of 500 characters

            builder.Property(u => u.Country)
                .HasMaxLength(100);

            builder.Property(u => u.YearsOfExperience)
                .HasDefaultValue(0); // Default value 0

            builder.Property(u => u.Level)
                .HasMaxLength(50);

            builder.Property(u => u.Image)
                .HasMaxLength(500);

            builder.Property(u => u.Price)
                .HasColumnType("decimal(18, 2)"); // Define precision for the decimal type

            // Relationships
            builder.HasMany(u => u.Quizzes)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.User_Id)
                .OnDelete(DeleteBehavior.NoAction); // Cascade delete if User is deleted

            builder.HasMany(u => u.Sessions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(u => u.Payments)
            //    .WithOne(p => p.User)
            //    .HasForeignKey(p => p.User_Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r=>r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Supports)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Badges)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Queries)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Skills)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.SocialAccounts)
                .WithOne(sa => sa.User)
                .HasForeignKey(sa => sa.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Schedules)
                .WithOne(sch => sch.User)
                .HasForeignKey(sch => sch.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Educations).WithOne(sch => sch.User).HasForeignKey(sch => sch.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Experiences).WithOne(sch => sch.User).HasForeignKey(sch => sch.User_Id)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

