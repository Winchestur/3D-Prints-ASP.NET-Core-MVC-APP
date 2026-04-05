using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;

namespace _3D_Prints_APP_Services
{
    public class PrinterService : IPrinterService
    {
        private readonly IPrinterRepository printerRepository;
        private readonly IPrinterOptionRepository printerOptionRepository;

        public PrinterService(IPrinterRepository printerRepository, IPrinterOptionRepository printerOptionRepository)
        {
            this.printerRepository = printerRepository;
            this.printerOptionRepository = printerOptionRepository;
        }

        public async Task<List<PrinterViewModel>> GetAllPrintersAsync(string userId)
        {
            IEnumerable<Printer> printers = await printerRepository.GetAllAsync(userId);

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

        public async Task CreatePrinterAsync(PrinterCreateFromOptionViewModel model, string userId)
        {
            var option = await printerOptionRepository.GetByIdAsync(model.PrinterOptionId);

            if (option == null)
            {
                return;
            }

            Printer printer = new Printer
            {
                ModelName = option.ModelName,
                NozzleDiameter = option.NozzleDiameter,
                Description = option.Description,
                UploadPhoto = option.UploadPhoto,
                AMS = option.AMS,
                UploadedTime = DateTime.Now,
                UserId = userId,
                PrinterOptionId = option.Id
            };

            await printerRepository.AddAsync(printer);
        }

        public async Task<PrinterCreateEditViewModel?> GetPrinterForEditAsync(int id, string userId)
        {
            Printer? printer = await printerRepository.GetByIdAsync(id, userId);

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

        public async Task<bool> UpdatePrinterAsync(int id, PrinterCreateEditViewModel model, string userId)
        {
            Printer? printer = await printerRepository.GetByIdAsync(id, userId);

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

        public async Task<Printer?> GetPrinterByIdAsync(int id, string userId)
        {
            return await printerRepository.GetByIdAsync(id, userId);
        }

        public async Task<bool> DeletePrinterAsync(int id, string userId)
        {
            var printer = await printerRepository.GetByIdWithFilamentsAsync(id, userId);

            if (printer == null)
            {
                return false;
            }

            await printerRepository.DeleteAsync(printer);
            await printerRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PrinterViewModel?> GetPrinterDetailsAsync(int id, string userId)
        {
            var printer = await printerRepository.GetByIdAsync(id, userId);

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
