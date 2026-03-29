using _3DPrintsAPP.Data.Configurations;
using _3DPrintsAPP.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
