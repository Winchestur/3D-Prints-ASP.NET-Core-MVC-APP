using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintFilamentConfiguration : IEntityTypeConfiguration<PrintFilament>
    {
        public void Configure(EntityTypeBuilder<PrintFilament> builder)
        {
            builder.HasKey(x => new { x.PrintId, x.FilamentId });

            builder
                .HasOne(pf => pf.Print)
                .WithMany(p => p.PrintFilaments)
                .HasForeignKey(pf => pf.PrintId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pf => pf.Filament)
                .WithMany(f => f.PrintFilaments)
                .HasForeignKey(pf => pf.FilamentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasData(
                new PrintFilament { PrintId = 1, FilamentId = 1 },
                new PrintFilament { PrintId = 1, FilamentId = 3 },

                new PrintFilament { PrintId = 2, FilamentId = 2 },

                new PrintFilament { PrintId = 3, FilamentId = 3 },

                new PrintFilament { PrintId = 4, FilamentId = 2 },
                new PrintFilament { PrintId = 4, FilamentId = 5 },

                new PrintFilament { PrintId = 5, FilamentId = 1 }
            );
        }
    }
}
