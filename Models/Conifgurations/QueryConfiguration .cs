using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;

namespace DevGuide.Models.Configrations
{
    //public class QueryConfiguration : IEntityTypeConfiguration<Query>
    //{
    //    public void Configure(EntityTypeBuilder<Query> builder)
    //    {
    //        builder.HasKey(q => q.Query_Id);
    //        builder.Property(q => q.Question)
    //            .IsRequired()
    //            .HasMaxLength(1000);
    //        builder.Property(q => q.File)
    //            .HasMaxLength(int.MaxValue);

    //        builder.HasOne(q => q.User)
    //            .WithMany(u => u.Queries)
    //            .HasForeignKey(q => q.User_Id).
    //            OnDelete(DeleteBehavior.NoAction);

    //        builder.HasOne(q => q.User_Instructor)
    //            .WithMany(q => q.Queries) // Assuming no navigation property in User
    //            .HasForeignKey(q => q.User_Instructor_Id)
    //            .OnDelete(DeleteBehavior.NoAction); // Disable cascade delete

    //        builder.HasMany(q => q.QueryAnswers)
    //            .WithOne(qa => qa.Query)
    //            .HasForeignKey(qa => qa.Answer_Id)
    //            .OnDelete(DeleteBehavior.NoAction);
    //    }
    //}

    public class QueryConfiguration : IEntityTypeConfiguration<Query>
    {
        public void Configure(EntityTypeBuilder<Query> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Question)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(s => s.DateTime).IsRequired();
            builder.Property(q => q.File)
                .HasMaxLength(int.MaxValue);

            // Map the relationship where the user is the creator
            builder.HasOne(q => q.User)
                .WithMany(u => u.Queries)
                .HasForeignKey(q => q.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Map the relationship where the user is the instructor
            builder.HasOne(q => q.User_Instructor)
                .WithMany(u => u.InstructedQueries)  // Use the new navigation property
                .HasForeignKey(q => q.User_Instructor_Id)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }

}
