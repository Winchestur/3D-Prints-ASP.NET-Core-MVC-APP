using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrinterConfiguration : IEntityTypeConfiguration<Printer>
    {
        public void Configure(EntityTypeBuilder<Printer> builder)
        {

            builder
                .Property(p => p.NozzleDiameter)
                .HasPrecision(18, 2);
        }
    }
}