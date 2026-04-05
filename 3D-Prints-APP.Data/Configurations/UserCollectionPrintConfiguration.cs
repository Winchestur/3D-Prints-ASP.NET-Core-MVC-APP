using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class UserCollectionPrintConfiguration : IEntityTypeConfiguration<UserCollectionPrint>
    {
        public void Configure(EntityTypeBuilder<UserCollectionPrint> builder)
        {
            builder.HasKey(x => new { x.UserId, x.PrintId });

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserCollectionPrints)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Print)
                .WithMany(p => p.UserCollectionPrints)
                .HasForeignKey(x => x.PrintId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}