using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;

namespace _3D_Prints_APP_Services
{
    public class PrinterService : IPrinterService
    {
        private readonly IPrinterRepository printerRepository;

        public PrinterService(IPrinterRepository printerRepository)
        {
            this.printerRepository = printerRepository;
        }

        public async Task<List<PrinterViewModel>> GetAllPrintersAsync()
        {
            IEnumerable<Printer> printers = await printerRepository.GetAllAsync();

            return printers
                .Select(p => new PrinterViewModel
                {
                    Id = p.Id,
                    ModelName = p.ModelName!,
                    NozzleDiameter = p.NozzleDiameter,
                    Description = p.Description!,
                    UploadPhoto = p.UploadPhoto!,
                    AMS = p.AMS,
                    UploadedTime = p.UploadedTime
                })
                .ToList();
        }

        public async Task CreatePrinterAsync(PrinterViewModel model)
        {
            Printer printer = new Printer
            {
                ModelName = model.ModelName,
                NozzleDiameter = model.NozzleDiameter,
                Description = model.Description,
                UploadPhoto = model.UploadPhoto,
                AMS = model.AMS,
                UploadedTime = DateTime.Now
            };

            await printerRepository.AddAsync(printer);
        }

        public async Task<PrinterCreateEditViewModel?> GetPrinterForEditAsync(int id)
        {
            Printer? printer = await printerRepository.GetByIdAsync(id);

            if (printer == null)
            {
                return null;
            }

            return new PrinterCreateEditViewModel
            {
                ModelName = printer.ModelName!,
                NozzleDiameter = printer.NozzleDiameter,
                Description = printer.Description!,
                UploadPhoto = printer.UploadPhoto!,
                AMS = printer.AMS
            };
        }

        public async Task<bool> UpdatePrinterAsync(int id, PrinterCreateEditViewModel model)
        {
            Printer printer = await printerRepository.GetByIdAsync(id);

            if (printer == null)
            {
                return false;
            }

            printer.ModelName = model.ModelName;
            printer.NozzleDiameter = model.NozzleDiameter;
            printer.Description = model.Description;
            printer.UploadPhoto = model.UploadPhoto;
            printer.AMS = model.AMS;

            await printerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<Printer?> GetPrinterByIdAsync(int id)
        {
            return await printerRepository.GetByIdAsync(id);
        }

        public async Task<bool> DeletePrinterAsync(int id)
        {
            var printer = await printerRepository.GetByIdWithFilamentsAsync(id);

            if (printer == null)
            {
                return false;
            }

            await printerRepository.DeleteAsync(printer);
            await printerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PrinterViewModel?> GetPrinterDetailsAsync(int id)
        {
            var printer = await printerRepository.GetByIdAsync(id);

            if (printer == null)
            {
                return null;
            }

            return new PrinterViewModel
            {
                Id = printer.Id,
                ModelName = printer.ModelName!,
                NozzleDiameter = printer.NozzleDiameter,
                Description = printer.Description!,
                UploadPhoto = printer.UploadPhoto!,
                AMS = printer.AMS,
                UploadedTime = printer.UploadedTime
            };
        }
    }
}
