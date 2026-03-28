using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3DPrintsAPP.Controllers
{
    public class PrintersController : Controller
    {
        private readonly IPrinterService printerService;

        public PrintersController(IPrinterService printerService)
        {
            this.printerService = printerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var printers = await printerService.GetAllPrintersAsync();
            return View(printers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrinterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await printerService.CreatePrinterAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await printerService.GetPrinterForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PrinterCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = await printerService.UpdatePrinterAsync(id, model);

            if (!isUpdated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Printer? printer = await printerService.GetPrinterByIdAsync(id);

            if (printer == null)
            {
                return NotFound();
            }

            return View(printer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await printerService.DeletePrinterAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await printerService.GetPrinterDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
