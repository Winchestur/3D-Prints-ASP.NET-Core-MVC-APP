using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.Enums;
using _3DPrintsAPP.ViewModels.Filaments;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP_Services
{
    public class FilamentService
    {
        private readonly IFilamentRepository filamentRepository;

        public FilamentService(IFilamentRepository filamentRepository)
        {
            this.filamentRepository = filamentRepository;
        }

        public async Task<ICollection<FilamentViewModel>> GetAllFilamentsAsync()
        {
            var filaments = await filamentRepository.GetAllWithPrinterAsync();
            return filaments.Select(f => new FilamentViewModel
            {
                Id = f.Id,
                Brand = f.Brand,
                Material = f.Material,
                FilamentColor = f.FilamentColor,
                UploadPhoto = f.UploadPhoto,
                WeightKg = f.WeightKG,
                Diameter = f.Diameter,
                PrinterId = f.PrinterId,
                PrinterModelName = f.Printer!.ModelName!
            }).ToList();
        }

        public async Task<FilamentViewModel?> GetFilamentDetailsAsync(int id)
        {
            var f = await filamentRepository.GetByIdWithPrinterAsync(id);
            if (f == null) return null;

            return new FilamentViewModel
            {
                Id = f.Id,
                Brand = f.Brand,
                Material = f.Material,
                FilamentColor = f.FilamentColor,
                UploadPhoto = f.UploadPhoto,
                WeightKg = f.WeightKG,
                Diameter = f.Diameter,
                PrinterId = f.PrinterId,
                PrinterModelName = f.Printer!.ModelName!
            };
        }

        public async Task<FilamentCreateEditViewModel> GetCreateViewModelAsync()
        {
            var printers = await filamentRepository.GetAllPrintersAsync();
            return new FilamentCreateEditViewModel
            {
                BrandOptions = Enum.GetValues(typeof(Brand)).Cast<Brand>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                    .ToList(),
                MaterialOptions = Enum.GetValues(typeof(Materials)).Cast<Materials>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                    .ToList(),
                ColorOptions = Enum.GetValues(typeof(Colors)).Cast<Colors>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                    .ToList(),
                PrinterOptions = printers.Select(p => new SelectListItem(p.ModelName!, p.Id.ToString()))
                    .ToList()
            };
        }

        public async Task CreateFilamentAsync(FilamentCreateEditViewModel model)
        {
            var f = new Filament
            {
                Brand = model.Brand,
                Material = model.Material,
                FilamentColor = model.FilamentColor,
                UploadPhoto = model.UploadPhoto,
                WeightKG = model.WeightKg,
                Diameter = model.Diameter,
                PrinterId = model.PrinterId
            };
            await filamentRepository.AddAsync(f);
        }

        public async Task<FilamentCreateEditViewModel?> GetEditViewModelAsync(int id)
        {
            var f = await filamentRepository.GetByIdWithPrinterAsync(id);
            if (f == null) return null;

            var printers = await filamentRepository.GetAllPrintersAsync();

            return new FilamentCreateEditViewModel
            {
                Brand = f.Brand,
                Material = f.Material,
                FilamentColor = f.FilamentColor,
                UploadPhoto = f.UploadPhoto,
                WeightKg = f.WeightKG,
                Diameter = f.Diameter,
                PrinterId = f.PrinterId,
                BrandOptions = Enum.GetValues(typeof(Brand)).Cast<Brand>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString())).ToList(),
                MaterialOptions = Enum.GetValues(typeof(Materials)).Cast<Materials>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString())).ToList(),
                ColorOptions = Enum.GetValues(typeof(Colors)).Cast<Colors>()
                    .Select(v => new SelectListItem(v.ToString(), v.ToString())).ToList(),
                PrinterOptions = printers.Select(p => new SelectListItem(p.ModelName!, p.Id.ToString())).ToList()
            };
        }

        public async Task EditFilamentAsync(int id, FilamentCreateEditViewModel model)
        {
            var f = await filamentRepository.GetByIdWithPrinterAsync(id);
            if (f == null) throw new KeyNotFoundException("Filament not found.");

            f.Brand = model.Brand;
            f.Material = model.Material;
            f.FilamentColor = model.FilamentColor;
            f.UploadPhoto = model.UploadPhoto;
            f.WeightKG = model.WeightKg;
            f.Diameter = model.Diameter;
            f.PrinterId = model.PrinterId;

            await filamentRepository.UpdateAsync(f);
        }

        public async Task DeleteFilamentAsync(int id)
        {
            var f = await filamentRepository.GetByIdWithPrinterAsync(id);
            if (f == null) throw new KeyNotFoundException("Filament not found.");

            await filamentRepository.DeleteAsync(f);
        }
    }
}
