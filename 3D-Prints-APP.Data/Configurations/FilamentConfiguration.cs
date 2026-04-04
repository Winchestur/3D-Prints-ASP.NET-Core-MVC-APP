using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace _3DPrintsAPP.Data.Configurations
{
    public class FilamentConfiguration : IEntityTypeConfiguration<Filament>
    {
        public void Configure(EntityTypeBuilder<Filament> builder)
        {
            builder
                .Property(f => f.Diameter)
                .HasPrecision(18, 2);

            builder
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(f => f.FilamentOption)
                .WithMany(f => f.Filaments)
                .HasForeignKey(f => f.FilamentOptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
