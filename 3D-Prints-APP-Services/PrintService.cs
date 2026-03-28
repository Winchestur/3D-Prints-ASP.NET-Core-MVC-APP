using _3D_Prints_APP.Data.Repositories.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Prints_APP_Services
{
    public class PrintService
    {
        private readonly IPrintRepository printRepository;

        public PrintService(IPrintRepository printRepository)
        {
            this.printRepository = printRepository;
        }

        public async Task<ICollection<PrintViewModel>> GetAllPrintsAsync()
        {
            var prints = await printRepository.GetAllWithPrinterAndFilamentsAsync();

            return prints.Select(p => new PrintViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description!,
                PrintTime = p.PrintTime,
                UploadPhoto = p.UploadPhoto!,
                UploadedTime = p.UploadedTime,
                PrinterId = p.PrinterId,
                PrinterModelName = p.Printer!.ModelName!,
                Filaments = p.PrintFilaments
                    .Select(f => f.Filament.Brand.ToString())
                    .ToList()
            }).ToList();
        }

        public async Task<PrintViewModel?> GetPrintDetailsAsync(int id)
        {
            Print? print = await printRepository.GetByIdWithPrinterAndFilamentsAsync(id);

            if (print == null)
                return null;

            return new PrintViewModel
            {
                Id = print.Id,
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                UploadedTime = print.UploadedTime,
                PrinterId = print.PrinterId,
                PrinterModelName = print.Printer!.ModelName!,
                Filaments = print.PrintFilaments
                    .Select(pf => $"{pf.Filament.Brand} {pf.Filament.Material} {pf.Filament.FilamentColor}")
                    .ToList()
            };
        }

        public async Task<PrintCreateEditViewModel> GetCreateViewModelAsync()
        {
            var printers = await printRepository.GetAllPrintersAsync();
            var filaments = await printRepository.GetAllFilamentsAsync();

            var viewModel = new PrintCreateEditViewModel
            {
                PrintTime = new TimeOnly(1, 0),
                PrinterOptions = printers.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.ModelName!
                }).ToList(),
                FilamentOptions = filaments.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = $"{f.Brand} {f.Material} {f.FilamentColor}"
                }).ToList()
            };

            return viewModel;
        }

        public async Task CreatePrintAsync(PrintCreateEditViewModel model)
        {
            var print = new Print
            {
                Title = model.Title,
                Description = model.Description,
                PrintTime = model.PrintTime,
                UploadPhoto = model.UploadPhoto,
                UploadedTime = DateTime.Now,
                PrinterId = model.PrinterId
            };

            await printRepository.AddPrintAsync(print);

            var printFilaments = model.SelectedFilamentIds
                .Distinct()
                .Select(filamentId => new PrintFilament
                {
                    PrintId = print.Id,
                    FilamentId = filamentId
                });

            await printRepository.AddPrintFilamentsAsync(printFilaments);
        }

        public async Task<PrintCreateEditViewModel?> GetEditViewModelAsync(int id)
        {
            var print = await printRepository.GetByIdWithFilamentsAsync(id);

            if (print == null)
                return null;

            return new PrintCreateEditViewModel
            {
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                PrinterId = print.PrinterId,
                SelectedFilamentIds = print.PrintFilaments
                    .Select(pf => pf.FilamentId)
                    .ToList()
            };
        }

        public async Task EditPrintAsync(int id, PrintCreateEditViewModel model)
        {
            var print = await printRepository.GetByIdWithFilamentsAsync(id);

            if (print == null)
                throw new KeyNotFoundException("Print not found.");

            // update main properties
            print.Title = model.Title;
            print.Description = model.Description;
            print.PrintTime = model.PrintTime;
            print.UploadPhoto = model.UploadPhoto;
            print.PrinterId = model.PrinterId;

            await printRepository.UpdatePrintAsync(print);

            // sync many-to-many Filaments
            var existingIds = print.PrintFilaments.Select(pf => pf.FilamentId).ToHashSet();
            var newIds = model.SelectedFilamentIds.Distinct().ToHashSet();

            var toRemove = print.PrintFilaments
                .Where(pf => !newIds.Contains(pf.FilamentId))
                .ToList();
            if (toRemove.Any())
                await printRepository.RemovePrintFilamentsAsync(toRemove);

            var toAdd = newIds.Where(fid => !existingIds.Contains(fid))
                              .Select(fid => new PrintFilament
                              {
                                  PrintId = print.Id,
                                  FilamentId = fid
                              }).ToList();

            if (toAdd.Any())
                await printRepository.AddPrintFilamentsAsync(toAdd);
        }

        public async Task<PrintViewModel?> GetDeleteViewModelAsync(int id)
        {
            var print = await printRepository.GetByIdWithPrinterAndFilamentsAsync(id);

            if (print == null)
                return null;

            return new PrintViewModel
            {
                Id = print.Id,
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                UploadedTime = print.UploadedTime,
                PrinterId = print.PrinterId,
                PrinterModelName = print.Printer!.ModelName!,
                Filaments = print.PrintFilaments
                    .Select(pf => $"{pf.Filament.Brand} {pf.Filament.Material} {pf.Filament.FilamentColor}")
                    .ToList()
            };
        }
        public async Task DeletePrintAsync(int id)
        {
            var print = await printRepository.GetByIdWithFilamentsAsync(id);
            if (print == null)
                throw new KeyNotFoundException("Print not found.");

            await printRepository.DeletePrintAsync(print);
        }
    }
}
