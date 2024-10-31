using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Conifgurations
{
    public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.User)
                .WithMany(ub => ub.Experiences)
                .HasForeignKey(ub => ub.User_Id).OnDelete(DeleteBehavior.NoAction);

        }
    }
    
}
