using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class PrinterOptionsController : Controller
    {
        private readonly IPrinterOptionService printerOptionService;

        public PrinterOptionsController(IPrinterOptionService printerOptionService)
        {
            this.printerOptionService = printerOptionService;
        }

        private bool IsAdmin()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) == "admin-user-id";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var options = await printerOptionService.GetAllAsync();
            return View(options);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrinterOption model)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await printerOptionService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var option = await printerOptionService.GetByIdAsync(id);

            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PrinterOption model)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = await printerOptionService.UpdateAsync(id, model);

            if (!isUpdated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var option = await printerOptionService.GetByIdAsync(id);

            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            bool isDeleted = await printerOptionService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}