using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrinterOptionConfiguration : IEntityTypeConfiguration<PrinterOption>
    {
        public void Configure(EntityTypeBuilder<PrinterOption> builder)
        {
            builder.Property(p => p.NozzleDiameter)
                .HasPrecision(18, 2);

            builder.HasData(
                new PrinterOption
                {
                    Id = 1,
                    ModelName = "Bambu Lab P1S",
                    NozzleDiameter = 1.75m,
                    Description = "Bambu Lab P1S with AMS for multi-color prints.",
                    UploadPhoto = "https://store.bblcdn.com/s7/default/7abb7477d233498b82a02dc93dd069df/P1.jpg",
                    AMS = true,
                    UploadedTime = new DateTime(2026, 02, 17, 0, 0, 0)
                },
                new PrinterOption
                {
                    Id = 2,
                    ModelName = "Creality Ender 3",
                    NozzleDiameter = 1.75m,
                    Description = "Reliable budget printer for starters and upgrades.",
                    UploadPhoto = "https://m.media-amazon.com/images/I/61CndtGd6wL._AC_UF350,350_QL80_.jpg",
                    AMS = false,
                    UploadedTime = new DateTime(2026, 02, 17, 0, 0, 0)
                }
            );
        }
    }
}