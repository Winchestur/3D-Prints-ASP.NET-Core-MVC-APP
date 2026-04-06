using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintRatingConfiguration : IEntityTypeConfiguration<PrintRating>
    {
        public void Configure(EntityTypeBuilder<PrintRating> builder)
        {
            builder.HasKey(x => new { x.PrintId, x.UserId });

            builder.Property(x => x.Value)
                .IsRequired();

            builder.HasOne(x => x.Print)
                .WithMany(p => p.Ratings)
                .HasForeignKey(x => x.PrintId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(u => u.PrintRatings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}