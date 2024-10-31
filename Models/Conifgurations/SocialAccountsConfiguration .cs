using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevGuide.Models;
using DevGuide.Models.Models;


namespace DevGuide.Models.Configrations
{
    public class SocialAccountsConfiguration : IEntityTypeConfiguration<SocialAccounts>
    {
        public void Configure(EntityTypeBuilder<SocialAccounts> builder)
        {
            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.SocialName).IsRequired();

            builder.Property(sa => sa.SocialLink);



            builder.HasOne(usa => usa.User)
                .WithMany(usa => usa.SocialAccounts) // Assuming no navigation property in User
                .HasForeignKey(usa => usa.User_Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
