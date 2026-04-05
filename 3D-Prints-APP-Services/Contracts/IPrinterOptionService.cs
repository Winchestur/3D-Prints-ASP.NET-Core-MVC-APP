using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IPrinterOptionService
    {
        Task<List<PrinterOption>> GetAllAsync();
        Task CreateAsync(PrinterOption option);
        Task<PrinterOption?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, PrinterOption model);
        Task<bool> DeleteAsync(int id);
    }
}