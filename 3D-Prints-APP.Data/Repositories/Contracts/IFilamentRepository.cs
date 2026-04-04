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
        Task<ICollection<Filament>> GetAllAsync(string userId);
        Task<Filament?> GetByIdAsync(int id, string userId);
        Task AddAsync(Filament filament);
        Task UpdateAsync(Filament filament);
        Task DeleteAsync(Filament filament);
    }
}
