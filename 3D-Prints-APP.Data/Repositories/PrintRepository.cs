using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP.Data.Repositories
{
    public class PrintRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PrintRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Print>> GetAllWithPrinterAndFilamentsAsync()
        {
            return await dbContext.Prints
                .Include(p => p.Printer)
                .Include(pf => pf.PrintFilaments)
                    .ThenInclude(f => f.Filament)
                .ToListAsync();
        }

        public async Task<Print?> GetByIdWithPrinterAndFilamentsAsync(int id)
        {
            return await dbContext.Prints
                .Include(p => p.Printer)
                .Include(p => p.PrintFilaments)
                    .ThenInclude(pf => pf.Filament)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<Printer>> GetAllPrintersAsync()
        {
            return await dbContext.Printers.ToListAsync();
        }

        public async Task<ICollection<Filament>> GetAllFilamentsAsync()
        {
            return await dbContext.Filaments.ToListAsync();
        }

        public async Task AddPrintAsync(Print print)
        {
            dbContext.Prints.Add(print);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddPrintFilamentsAsync(IEnumerable<PrintFilament> printFilaments)
        {
            dbContext.PrintFilaments.AddRange(printFilaments);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Print?> GetByIdWithFilamentsAsync(int id)
        {
            return await dbContext.Prints
                .Include(p => p.PrintFilaments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePrintAsync(Print print)
        {
            dbContext.Prints.Update(print);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemovePrintFilamentsAsync(IEnumerable<PrintFilament> printFilaments)
        {
            dbContext.PrintFilaments.RemoveRange(printFilaments);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeletePrintAsync(Print print)
        {
            dbContext.PrintFilaments.RemoveRange(print.PrintFilaments);
            dbContext.Prints.Remove(print);
            await dbContext.SaveChangesAsync();
        }
    }
}
