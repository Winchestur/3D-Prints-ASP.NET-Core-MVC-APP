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
        Task<List<PrinterViewModel>> GetAllPrintersAsync();
        Task CreatePrinterAsync(PrinterViewModel model);
        Task<PrinterCreateEditViewModel?> GetPrinterForEditAsync(int id);
        Task<bool> UpdatePrinterAsync(int id, PrinterCreateEditViewModel model);
        Task<Printer?> GetPrinterByIdAsync(int id);
        Task<bool> DeletePrinterAsync(int id);
        Task<PrinterViewModel?> GetPrinterDetailsAsync(int id);
    }
}
