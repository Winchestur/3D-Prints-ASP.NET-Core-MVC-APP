using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP.Data.Repositories
{
    public class PrinterRepository : IPrinterRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrinterRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Printer>> GetAllAsync(string userId)
        {
            return await dbContext.Printers
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Printer printer)
        {
            await dbContext.Printers.AddAsync(printer);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Printer?> GetByIdAsync(int id, string userId)
        {
            return await dbContext.Printers
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<Printer?> GetByIdWithFilamentsAsync(int id, string userId)
        {
            return await dbContext.Printers
                .Include(p => p.Filaments)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task DeleteAsync(Printer printer)
        {
            dbContext.Filaments.RemoveRange(printer.Filaments);
            dbContext.Printers.Remove(printer);

            await Task.CompletedTask;
        }
    }
}
