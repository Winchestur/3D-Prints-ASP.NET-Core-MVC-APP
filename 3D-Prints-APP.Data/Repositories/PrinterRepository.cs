using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP.Data.Repositories
{
    public class PrinterRepository : IPrinterRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrinterRepository(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Printer>> GetAllAsync()
        {
            return await dbContext.Printers.ToListAsync();
        }

        public async Task AddAsync(Printer printer)
        {
            await dbContext.Printers.AddAsync(printer);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Printer?> GetByIdAsync(int id)
        {
            return await dbContext.Printers.FindAsync(id);
        }

        // its used for Edit/Delete
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<Printer?> GetByIdWithFilamentsAsync(int id)
        {
            return await dbContext.Printers
                .Include(p => p.Filaments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task DeleteAsync(Printer printer)
        {
            dbContext.Filaments.RemoveRange(printer.Filaments);
            dbContext.Printers.Remove(printer);

            await Task.CompletedTask;
        }
    }
}
