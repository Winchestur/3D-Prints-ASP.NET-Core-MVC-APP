using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP.Data.Repositories.Contracts
{
    public interface IPrinterOptionRepository
    {
        Task<List<PrinterOption>> GetAllAsync();
        Task<PrinterOption?> GetByIdAsync(int id);
        Task AddAsync(PrinterOption option);
        Task SaveChangesAsync();
        Task DeleteAsync(PrinterOption option);
    }
}