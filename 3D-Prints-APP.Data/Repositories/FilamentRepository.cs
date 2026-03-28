using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP_Services
{
    public class FilamentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FilamentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Filament>> GetAllWithPrinterAsync()
        {
            return await dbContext.Filaments
                .Include(f => f.Printer)
                .ToListAsync();
        }

        public async Task<Filament?> GetByIdWithPrinterAsync(int id)
        {
            return await dbContext.Filaments
                .Include(f => f.Printer)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync(Filament filament)
        {
            dbContext.Filaments.Add(filament);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Filament filament)
        {
            dbContext.Filaments.Update(filament);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Filament filament)
        {
            dbContext.Filaments.Remove(filament);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Printer>> GetAllPrintersAsync()
        {
            return await dbContext.Printers.ToListAsync();
        }
    }
}
