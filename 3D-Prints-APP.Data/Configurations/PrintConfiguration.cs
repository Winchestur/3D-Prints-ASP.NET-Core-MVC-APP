using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintConfiguration : IEntityTypeConfiguration<Print>
    {
        public void Configure(EntityTypeBuilder<Print> builder)
        {
            builder.Property(p => p.UploadPhoto)
                .HasMaxLength(2048);

            builder.Property(p => p.IsPublic)
                .HasDefaultValue(false);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Prints)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
