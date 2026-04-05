using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP_Services.Contracts
{
    public interface IPrinterService
    {
        Task<List<PrinterViewModel>> GetAllPrintersAsync(string userId);
        Task CreatePrinterAsync(PrinterCreateFromOptionViewModel model, string userId);
        Task<PrinterCreateEditViewModel?> GetPrinterForEditAsync(int id, string userId);
        Task<bool> UpdatePrinterAsync(int id, PrinterCreateEditViewModel model, string userId);
        Task<Printer?> GetPrinterByIdAsync(int id, string userId);
        Task<bool> DeletePrinterAsync(int id, string userId);
        Task<PrinterViewModel?> GetPrinterDetailsAsync(int id, string userId);
    }
}
