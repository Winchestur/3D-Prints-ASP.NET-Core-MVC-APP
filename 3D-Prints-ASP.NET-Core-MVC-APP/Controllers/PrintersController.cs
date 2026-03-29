using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels.Printers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class PrintersController : Controller
    {
        private readonly IPrinterService printerService;

        public PrintersController(IPrinterService printerService)
        {
            this.printerService = printerService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var printers = await printerService.GetAllPrintersAsync(GetUserId());
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

            await printerService.CreatePrinterAsync(model, GetUserId());

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await printerService.GetPrinterForEditAsync(id, GetUserId());

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

            bool isUpdated = await printerService.UpdatePrinterAsync(id, model, GetUserId());

            if (!isUpdated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Printer? printer = await printerService.GetPrinterByIdAsync(id, GetUserId());

            if (printer == null)
            {
                return NotFound();
            }

            return View(printer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isDeleted = await printerService.DeletePrinterAsync(id, GetUserId());

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await printerService.GetPrinterDetailsAsync(id, GetUserId());

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}