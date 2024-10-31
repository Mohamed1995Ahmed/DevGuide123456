using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Conifgurations
{
    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.User)
                .WithMany(ub => ub.Educations)
                .HasForeignKey(ub => ub.User_Id).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
