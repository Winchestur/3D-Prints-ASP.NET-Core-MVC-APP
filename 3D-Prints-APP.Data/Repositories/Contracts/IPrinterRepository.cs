using _3DPrintsAPP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP.Data.Repositories.Contracts
{
    public interface IPrinterRepository
    {
        Task<List<Printer>> GetAllAsync(string userId);
        Task AddAsync(Printer printer);
        Task<Printer?> GetByIdAsync(int id, string userId);
        Task SaveChangesAsync();
        Task<Printer?> GetByIdWithFilamentsAsync(int id, string userId);
        Task DeleteAsync(Printer printer);
    }
}
