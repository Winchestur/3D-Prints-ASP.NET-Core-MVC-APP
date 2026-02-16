using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;
using _3DPrintsASP.NETCoreMVCAPP.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _3DPrintsAPP.Controllers
{
    public class PrintsController : Controller
{
        private readonly ApplicationDbContext _logger;
        public PrintsController(ApplicationDbContext? logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<PrintViewModel> prints = _logger.Prints
                .Include(p => p.Printer)
                .Include(pf => pf.PrintFilaments)
                    .ThenInclude(f => f.Filament)
                .Select(p => new PrintViewModel
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
                })
                .ToList();

            return View(prints);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Print? print = _logger.Prints
                .Include(p => p.Printer)
                .Include(p => p.PrintFilaments)
                    .ThenInclude(pf => pf.Filament)
                .FirstOrDefault(p => p.Id == id);

            if (print == null)
            {
                return NotFound();
            }

            PrintViewModel viewModel = new PrintViewModel
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
                    .Select(pf =>
                        $"{pf.Filament.Brand} {pf.Filament.Material} {pf.Filament.FilamentColor}")
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PrintCreateEditViewModel? viewModel = new PrintCreateEditViewModel
            {
                PrintTime = new TimeOnly(1, 0)
            };

            FillDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(PrintCreateEditViewModel model)
        {
            if (model.SelectedFilamentIds == null || model.SelectedFilamentIds.Count == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedFilamentIds), "Select at least one filament.");
            }

            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            Print? print = new Print
            {
                Title = model.Title,
                Description = model.Description,
                PrintTime = model.PrintTime,
                UploadPhoto = model.UploadPhoto,
                UploadedTime = DateTime.Now,
                PrinterId = model.PrinterId
            };

            _logger.Prints.Add(print);
            _logger.SaveChanges();

            foreach (var filamentId in model.SelectedFilamentIds.Distinct())
            {
                _logger.PrintFilaments.Add(new PrintFilament
                {
                    PrintId = print.Id,
                    FilamentId = filamentId
                });
            }

            _logger.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Print? print = _logger.Prints
                .Include(p => p.PrintFilaments)
                .FirstOrDefault(p => p.Id == id);

            if (print == null)
            {
                return NotFound();
            }

            PrintCreateEditViewModel viewModel = new PrintCreateEditViewModel
            {
                Title = print.Title,
                Description = print.Description!,
                PrintTime = print.PrintTime,
                UploadPhoto = print.UploadPhoto!,
                PrinterId = print.PrinterId,
                SelectedFilamentIds = print.PrintFilaments
                .Select(pf => pf.FilamentId).ToList()
            };

            FillDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, PrintCreateEditViewModel model)
        {
            if (model.SelectedFilamentIds == null || model.SelectedFilamentIds.Count == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedFilamentIds), "Select at least one filament.");
            }

            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            var print = _logger.Prints
                .Include(p => p.PrintFilaments)
                .FirstOrDefault(p => p.Id == id);

            if (print == null)
            {
                return NotFound();
            }

            print.Title = model.Title;
            print.Description = model.Description;
            print.PrintTime = model.PrintTime;
            print.UploadPhoto = model.UploadPhoto;
            print.PrinterId = model.PrinterId;

            // sync many-to-many
            var existingIds = print.PrintFilaments.Select(pf => pf.FilamentId).ToHashSet();
            var newIds = model.SelectedFilamentIds.Distinct().ToHashSet();

            // remove old
            var toRemove = print.PrintFilaments
                .Where(pf => !newIds.Contains(pf.FilamentId)).ToList();
            
            _logger.PrintFilaments.RemoveRange(toRemove);

            // add new
            var toAdd = newIds.Where(fid => !existingIds.Contains(fid));
            
            foreach (var filamentId in toAdd)
            {
                _logger.PrintFilaments.Add(new PrintFilament
                {
                    PrintId = print.Id,
                    FilamentId = filamentId
                });
            }

            _logger.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Print? print = _logger.Prints
                .Include(p => p.Printer)
                .Include(p => p.PrintFilaments)
                    .ThenInclude(pf => pf.Filament)
                .FirstOrDefault(p => p.Id == id);

            if (print == null)
            {
                return NotFound();
            }

            PrintViewModel viewModel = new PrintViewModel
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

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Print? print = _logger.Prints
                .Include(p => p.PrintFilaments)
                .FirstOrDefault(p => p.Id == id);

            if (print == null)
            {
                return NotFound();
            }

            _logger.PrintFilaments.RemoveRange(print.PrintFilaments);

            _logger.Prints.Remove(print);

            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        private void FillDropdowns(PrintCreateEditViewModel viewModel)
        {
            viewModel.PrinterOptions = _logger.Printers
                .Select(p => new SelectListItem(p.ModelName!, p.Id.ToString()))
                .ToList();

            viewModel.FilamentOptions = _logger.Filaments
                .Select(f => new SelectListItem(
                    $"{f.Brand} {f.Material} {f.FilamentColor} • {f.WeightKG}kg • {f.Diameter}mm",
                    f.Id.ToString()))
                .ToList();
        }
    }
}
