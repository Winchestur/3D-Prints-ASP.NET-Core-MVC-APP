using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3DPrintsAPP.Controllers
{
    public class PrintersController : Controller
    {
        private readonly ApplicationDbContext? dbContext;
        public PrintersController(ApplicationDbContext? dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<PrinterViewModel> printers = dbContext!
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
            dbContext!.Printers.Add(printer);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Printer? printer = dbContext?
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

            Printer? printer = dbContext?
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

            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var printer = dbContext?
                .Printers
                .Find(id);

            if (printer == null)
            {
               return NotFound();
            }

            return View(printer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var printer = dbContext.Printers
                .Include(p => p.Filaments)
                .FirstOrDefault(p => p.Id == id);

            if (printer == null)
                return NotFound();

            dbContext.Filaments.RemoveRange(printer.Filaments);
            dbContext.Printers.Remove(printer);

            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var printer = dbContext?
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
