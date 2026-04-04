using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Filaments;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _3D_Prints_APP_Services
{
    public class FilamentService : IFilamentService
    {
        private readonly IFilamentRepository filamentRepository;
        private readonly IFilamentOptionService filamentOptionService;

        public FilamentService(
            IFilamentRepository filamentRepository,
            IFilamentOptionService filamentOptionService)
        {
            this.filamentRepository = filamentRepository;
            this.filamentOptionService = filamentOptionService;
        }

        public async Task<ICollection<FilamentViewModel>> GetAllFilamentsAsync(string userId)
        {
            var filaments = await filamentRepository.GetAllAsync(userId);

            return filaments.Select(f => new FilamentViewModel
            {
                Id = f.Id,
                Brand = f.Brand,
                Material = f.Material,
                FilamentColor = f.FilamentColor,
                UploadPhoto = f.UploadPhoto,
                WeightKg = f.WeightKG,
                Diameter = f.Diameter
            }).ToList();
        }

        public async Task<FilamentViewModel?> GetFilamentDetailsAsync(int id, string userId)
        {
            var f = await filamentRepository.GetByIdAsync(id, userId);
            if (f == null) return null;

            return new FilamentViewModel
            {
                Id = f.Id,
                Brand = f.Brand,
                Material = f.Material,
                FilamentColor = f.FilamentColor,
                UploadPhoto = f.UploadPhoto,
                WeightKg = f.WeightKG,
                Diameter = f.Diameter
            };
        }

        public async Task<FilamentCreateEditViewModel> GetCreateViewModelAsync()
        {
            var filamentOptions = await filamentOptionService.GetAllAsync();

            return new FilamentCreateEditViewModel
            {
                FilamentOptions = filamentOptions.Select(f => new SelectListItem(
                    $"{f.Brand} - {f.Material} - {f.FilamentColor}",
                    f.Id.ToString()))
                    .ToList()
            };
        }

        public async Task CreateFilamentAsync(FilamentCreateEditViewModel model, string userId)
        {
            var option = await filamentOptionService.GetByIdAsync(model.FilamentOptionId);

            if (option == null)
            {
                throw new KeyNotFoundException("Filament option not found.");
            }

            var f = new Filament
            {
                Brand = option.Brand,
                Material = option.Material,
                FilamentColor = option.FilamentColor,
                UploadPhoto = option.UploadPhoto,
                WeightKG = option.WeightKG,
                Diameter = option.Diameter,
                UserId = userId,
                FilamentOptionId = option.Id
            };

            await filamentRepository.AddAsync(f);
        }

        public async Task<FilamentCreateEditViewModel?> GetEditViewModelAsync(int id, string userId)
        {
            var f = await filamentRepository.GetByIdAsync(id, userId);
            if (f == null) return null;

            var filamentOptions = await filamentOptionService.GetAllAsync();

            return new FilamentCreateEditViewModel
            {
                FilamentOptionId = f.FilamentOptionId,
                FilamentOptions = filamentOptions.Select(opt => new SelectListItem(
                    $"{opt.Brand} - {opt.Material} - {opt.FilamentColor}",
                    opt.Id.ToString()))
                    .ToList()
            };
        }

        public async Task EditFilamentAsync(int id, FilamentCreateEditViewModel model, string userId)
        {
            var f = await filamentRepository.GetByIdAsync(id, userId);
            if (f == null) throw new KeyNotFoundException("Filament not found.");

            var option = await filamentOptionService.GetByIdAsync(model.FilamentOptionId);
            if (option == null) throw new KeyNotFoundException("Filament option not found.");

            f.Brand = option.Brand;
            f.Material = option.Material;
            f.FilamentColor = option.FilamentColor;
            f.UploadPhoto = option.UploadPhoto;
            f.WeightKG = option.WeightKG;
            f.Diameter = option.Diameter;
            f.FilamentOptionId = option.Id;

            await filamentRepository.UpdateAsync(f);
        }

        public async Task DeleteFilamentAsync(int id, string userId)
        {
            var f = await filamentRepository.GetByIdAsync(id, userId);
            if (f == null) throw new KeyNotFoundException("Filament not found.");

            await filamentRepository.DeleteAsync(f);
        }
    }
}