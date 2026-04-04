using _3D_Prints_APP.Data.Repositories.Contracts;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;

namespace _3D_Prints_APP_Services
{
    public class FilamentOptionService : IFilamentOptionService
    {
        private readonly IFilamentOptionRepository filamentOptionRepository;

        public FilamentOptionService(IFilamentOptionRepository filamentOptionRepository)
        {
            this.filamentOptionRepository = filamentOptionRepository;
        }

        public async Task<List<FilamentOption>> GetAllAsync()
        {
            return await filamentOptionRepository.GetAllAsync();
        }

        public async Task<FilamentOption?> GetByIdAsync(int id)
        {
            return await filamentOptionRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(FilamentOption option)
        {
            await filamentOptionRepository.AddAsync(option);
        }

        public async Task<bool> UpdateAsync(int id, FilamentOption model)
        {
            var option = await filamentOptionRepository.GetByIdAsync(id);

            if (option == null)
            {
                return false;
            }

            option.Brand = model.Brand;
            option.Material = model.Material;
            option.FilamentColor = model.FilamentColor;
            option.UploadPhoto = model.UploadPhoto;
            option.WeightKG = model.WeightKG;
            option.Diameter = model.Diameter;

            await filamentOptionRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var option = await filamentOptionRepository.GetByIdAsync(id);

            if (option == null)
            {
                return false;
            }

            await filamentOptionRepository.DeleteAsync(option);
            await filamentOptionRepository.SaveChangesAsync();
            return true;
        }
    }
}