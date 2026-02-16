using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Enums;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;

namespace _3DPrintsAPP.Controllers
{
    public class FilamentsController : Controller
    {
        private readonly ApplicationDbContext _logger;

        public FilamentsController(ApplicationDbContext logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ICollection<FilamentViewModel> filaments = _logger
                .Filaments
                .Include(f => f.Printer)
                .Select(filament => new FilamentViewModel
                {
                    Id = filament.Id,
                    Brand = filament.Brand,
                    Material = filament.Material,
                    FilamentColor = filament.FilamentColor,
                    UploadPhoto = filament.UploadPhoto,
                    WeightKg = filament.WeightKG,
                    Diameter = filament.Diameter,
                    PrinterId = filament.PrinterId,
                    PrinterModelName = filament.Printer.ModelName!
                })
                .ToList();

            return View(filaments);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Filament? filament = _logger
                .Filaments
                .Include(x => x.Printer)
                .FirstOrDefault(x => x.Id == id);

            if (filament == null)
            {
                return NotFound();
            }

            FilamentViewModel viewModel = new FilamentViewModel
            {
                Id = filament.Id,
                Brand = filament.Brand,
                Material = filament.Material,
                FilamentColor = filament.FilamentColor,
                UploadPhoto = filament.UploadPhoto,
                WeightKg = filament.WeightKG,
                Diameter = filament.Diameter,
                PrinterId = filament.PrinterId,
                PrinterModelName = filament.Printer.ModelName!
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FilamentCreateEditViewModel viewModel = new FilamentCreateEditViewModel();
            FillDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(FilamentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            Filament filament = new Filament
            {
                Brand = model.Brand,
                Material = model.Material,
                FilamentColor = model.FilamentColor,
                UploadPhoto = model.UploadPhoto,
                WeightKG = model.WeightKg,
                Diameter = model.Diameter,
                PrinterId = model.PrinterId
            };

            _logger.Filaments.Add(filament);
            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Filament? filament = _logger
                .Filaments
                .Find(id);

            if (filament == null)
            {
                return NotFound();
            }

            FilamentCreateEditViewModel viewModel = new FilamentCreateEditViewModel
            {
                Brand = filament.Brand,
                Material = filament.Material,
                FilamentColor = filament.FilamentColor,
                UploadPhoto = filament.UploadPhoto,
                WeightKg = filament.WeightKG,
                Diameter = filament.Diameter,
                PrinterId = filament.PrinterId
            };

            FillDropdowns(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, FilamentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            Filament? filament = _logger
                .Filaments
                .Find(id);

            if (filament == null)
            {
                return NotFound();
            }

            filament.Brand = model.Brand;
            filament.Material = model.Material;
            filament.FilamentColor = model.FilamentColor;
            filament.UploadPhoto = model.UploadPhoto;
            filament.WeightKG = model.WeightKg;
            filament.Diameter = model.Diameter;
            filament.PrinterId = model.PrinterId;

            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Filament? filament = _logger.Filaments
                .Include(x => x.Printer)
                .FirstOrDefault(x => x.Id == id);

            if (filament == null)
            {
                return NotFound();
            }

            FilamentViewModel viewModel = new FilamentViewModel
            {
                Id = filament.Id,
                Brand = filament.Brand,
                Material = filament.Material,
                FilamentColor = filament.FilamentColor,
                UploadPhoto = filament.UploadPhoto,
                WeightKg = filament.WeightKG,
                Diameter = filament.Diameter,
                PrinterId = filament.PrinterId,
                PrinterModelName = filament.Printer.ModelName!
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Filament? filament = _logger
                .Filaments
                .Find(id);

            if (filament == null)
            {
                return NotFound();
            }

            _logger.Filaments.Remove(filament);
            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void FillDropdowns(FilamentCreateEditViewModel viewModel)
        {
            viewModel.BrandOptions = Enum.GetValues(typeof(Brand))
                .Cast<Brand>()
                .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                .ToList();

            viewModel.MaterialOptions = Enum.GetValues(typeof(Materials))
                .Cast<Materials>()
                .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                .ToList();

            viewModel.ColorOptions = Enum.GetValues(typeof(Colors))
                .Cast<Colors>()
                .Select(v => new SelectListItem(v.ToString(), v.ToString()))
                .ToList();

            viewModel.PrinterOptions = _logger.Printers
                .Select(p => new SelectListItem(p.ModelName!, p.Id.ToString()))
                .ToList();
        }
    }
}
