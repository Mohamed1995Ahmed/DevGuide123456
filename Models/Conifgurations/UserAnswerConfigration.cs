using DevGuide.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevGuide.Models.Models;

namespace DevGuide.Models.Configrations
{
    public class UserAnswerConfigration :IEntityTypeConfiguration<User_Answer>
   {
  
     public void Configure(EntityTypeBuilder<User_Answer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(s => s.User_Quiz)
               .WithMany(u => u.Answers)
               .HasForeignKey(s => s.User_Quiz_Id).
               OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(s => s.Question)
               .WithMany(u => u.User_Answers)
               .HasForeignKey(s => s.Question_Id).
               OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(s => s.Option)
              .WithMany(u => u.UserAnswer)
              .HasForeignKey(s => s.Option_Id).
              OnDelete(DeleteBehavior.NoAction);

    }
}
}




