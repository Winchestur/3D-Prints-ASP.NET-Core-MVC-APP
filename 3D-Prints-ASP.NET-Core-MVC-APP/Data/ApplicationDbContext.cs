using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _3DPrintsAPP.Data.Models;

namespace _3DPrintsAPP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Print> Prints { get; set; } = null!;
        public DbSet<Printer> Printers { get; set; } = null!;
        public DbSet<Filament> Filaments { get; set; } = null!;
        public DbSet<PrintFilament> PrintFilaments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrintFilament>()
                .HasKey(x => new { x.PrintId, x.FilamentId });

            modelBuilder.Entity<PrintFilament>()
                .HasOne(pf => pf.Print)
                .WithMany(p => p.PrintFilaments)
                .HasForeignKey(pf => pf.PrintId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrintFilament>()
                .HasOne(pf => pf.Filament)
                .WithMany(f => f.PrintFilaments)
                .HasForeignKey(pf => pf.FilamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrintFilament>()
                .Property(pf => pf.Cost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PrintFilament>()
                .Property(pf => pf.UsedGrams)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Filament>()
                .Property(f => f.Diameter)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Printer>()
                .Property(p => p.NozzleDiameter)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Filament>()
                .HasOne(f => f.Printer)
                .WithMany(f => f.Filaments)
                .HasForeignKey(f => f.PrinterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Print>()
                .HasOne(p => p.Printer)
                .WithMany(pr => pr.Prints)
                .HasForeignKey(p => p.PrinterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
