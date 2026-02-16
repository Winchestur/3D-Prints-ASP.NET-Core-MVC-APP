using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3DPrintsAPP.Controllers
{
    public class PrintersController : Controller
    {
        private readonly ApplicationDbContext? _logger;
        public PrintersController(ApplicationDbContext? logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<PrinterViewModel> printers = _logger!
            .Printers
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

            return View(printers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PrinterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            Printer printer = new Printer
            {
                ModelName = model.ModelName,
                NozzleDiameter = model.NozzleDiameter,
                Description = model.Description,
                UploadPhoto = model.UploadPhoto,
                AMS = model.AMS,
                UploadedTime = DateTime.Now
            };
            _logger!.Printers.Add(printer);
            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Printer? printer = _logger?
                .Printers
                .Find(id);

            if (printer == null)
            {
                return NotFound();
            }

            var model = new PrinterCreateEditViewModel
            {
                ModelName = printer.ModelName!,
                NozzleDiameter = printer.NozzleDiameter,
                Description = printer.Description!,
                UploadPhoto = printer.UploadPhoto!,
                AMS = printer.AMS
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, PrinterCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Printer? printer = _logger?
                .Printers
                .Find(id);

            if (printer == null)
            {
                return NotFound();
            }

            printer.ModelName = model.ModelName;
            printer.NozzleDiameter = model.NozzleDiameter;
            printer.Description = model.Description;
            printer.UploadPhoto = model.UploadPhoto;
            printer.AMS = model.AMS;

            _logger.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var printer = _logger?
                .Printers
                .Find(id);

            if (printer == null)
            {
               return NotFound();
            }

            return View(printer);
        }

        [HttpPost]
        public IActionResult Delete(int id, Printer model)
        {
            Printer? printer = _logger?
                .Printers
                .Find(id);

            if (printer == null)
            {
                return NotFound();
            }

            _logger?.Printers.Remove(printer);
            _logger?.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var printer = _logger?
                .Printers
                .Find(id);

            if (printer == null)
            {
                return NotFound();
            }

            PrinterViewModel viewModel = new PrinterViewModel
            {
                Id = printer.Id,
                ModelName = printer.ModelName!,
                NozzleDiameter = printer.NozzleDiameter,
                Description = printer.Description!,
                UploadPhoto = printer.UploadPhoto!,
                AMS = printer.AMS,
                UploadedTime = printer.UploadedTime
            };

            return View(viewModel);
        }
    }
}
