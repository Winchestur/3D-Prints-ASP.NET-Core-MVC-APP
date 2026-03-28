using _3DPrintsAPP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP.Data.Repositories.Contracts
{
    public interface IPrintRepository
    {
        Task<ICollection<Print>> GetAllWithPrinterAndFilamentsAsync();
        Task<Print?> GetByIdWithPrinterAndFilamentsAsync(int id);
        Task<ICollection<Printer>> GetAllPrintersAsync();
        Task<ICollection<Filament>> GetAllFilamentsAsync();
        Task AddPrintAsync(Print print);
        Task AddPrintFilamentsAsync(IEnumerable<PrintFilament> printFilaments);
        Task<Print?> GetByIdWithFilamentsAsync(int id);
        Task UpdatePrintAsync(Print print);
        Task RemovePrintFilamentsAsync(IEnumerable<PrintFilament> printFilaments);
        Task DeletePrintAsync(Print print);
    }
}
