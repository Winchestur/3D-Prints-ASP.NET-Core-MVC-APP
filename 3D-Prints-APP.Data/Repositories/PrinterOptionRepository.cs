using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _3D_Prints_APP.Data.Repositories
{
    public class PrinterOptionRepository : IPrinterOptionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrinterOptionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<PrinterOption>> GetAllAsync()
        {
            return await dbContext.PrinterOptions.ToListAsync();
        }

        public async Task<PrinterOption?> GetByIdAsync(int id)
        {
            return await dbContext.PrinterOptions.FindAsync(id);
        }

        public async Task AddAsync(PrinterOption option)
        {
            await dbContext.PrinterOptions.AddAsync(option);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(PrinterOption option)
        {
            dbContext.PrinterOptions.Remove(option);
            await Task.CompletedTask;
        }
    }
}