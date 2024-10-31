using DevGuide.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGuide.Models.Configrations
{
    public class UserQuizConfigration : IEntityTypeConfiguration<User_Quiz>
    {
        public void Configure(EntityTypeBuilder<User_Quiz> builder)
        {
           builder.HasKey(x => x.Id);
            builder.Property(x => x.Score).IsRequired();
            builder.Property(x=>x.Result).IsRequired();
            builder.Property(x => x.QuizCreated).IsRequired();
            builder.HasOne(x=>x.User).WithMany(x=>x.Quizzes).HasForeignKey(x=>x.User_Id).OnDelete(DeleteBehavior.NoAction);  
            builder.HasOne(x=>x.Quiz).WithMany(x=>x.Users).HasForeignKey(x=>x.Quiz_Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.Answers).WithOne(x => x.User_Quiz).HasForeignKey(x => x.User_Quiz_Id).OnDelete(DeleteBehavior.NoAction);
        }

      
    }
    
    
    
}
