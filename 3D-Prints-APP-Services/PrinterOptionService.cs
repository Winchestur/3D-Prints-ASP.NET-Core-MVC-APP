using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP_Services
{
    public class PrinterOptionService : IPrinterOptionService
    {
        private readonly IPrinterOptionRepository printerOptionRepository;

        public PrinterOptionService(IPrinterOptionRepository printerOptionRepository)
        {
            this.printerOptionRepository = printerOptionRepository;
        }

        public async Task<List<PrinterOption>> GetAllAsync()
        {
            return await printerOptionRepository.GetAllAsync();
        }

        public async Task CreateAsync(PrinterOption option)
        {
            await printerOptionRepository.AddAsync(option);
        }

        public async Task<PrinterOption?> GetByIdAsync(int id)
        {
            return await printerOptionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, PrinterOption model)
        {
            var option = await printerOptionRepository.GetByIdAsync(id);

            if (option == null)
            {
                return false;
            }

            option.ModelName = model.ModelName;
            option.NozzleDiameter = model.NozzleDiameter;
            option.Description = model.Description;
            option.UploadPhoto = model.UploadPhoto;
            option.AMS = model.AMS;

            await printerOptionRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var option = await printerOptionRepository.GetByIdAsync(id);

            if (option == null)
            {
                return false;
            }

            await printerOptionRepository.DeleteAsync(option);
            await printerOptionRepository.SaveChangesAsync();
            return true;
        }
    }
}