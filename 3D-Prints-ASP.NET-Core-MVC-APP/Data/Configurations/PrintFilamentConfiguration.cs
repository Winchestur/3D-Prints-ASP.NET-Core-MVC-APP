using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using _3DPrintsAPP.Data.Models;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintFilamentConfiguration : IEntityTypeConfiguration<PrintFilament>
    {
        public void Configure(EntityTypeBuilder<PrintFilament> builder)
        {
            builder.HasKey(x => new { x.PrintId, x.FilamentId });

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
