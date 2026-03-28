using _3DPrintsAPP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IFilamentRepository
    {
        Task<ICollection<Filament>> GetAllWithPrinterAsync();
        Task<Filament?> GetByIdWithPrinterAsync(int id);
        Task AddAsync(Filament filament);
        Task UpdateAsync(Filament filament);
        Task DeleteAsync(Filament filament);
        Task<ICollection<Printer>> GetAllPrintersAsync();
    }
}
