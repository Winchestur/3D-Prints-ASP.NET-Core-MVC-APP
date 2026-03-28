using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace _3DPrintsAPP.Controllers
{
    public class PrintsController : Controller
{
        private readonly IPrintService printService;
        public PrintsController(IPrintService printService)
        {
            this.printService = printService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ICollection<PrintViewModel> prints = await printService.GetAllPrintsAsync();
            return View(prints);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            PrintViewModel? viewModel = await printService.GetPrintDetailsAsync(id);

            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await printService.GetCreateViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrintCreateEditViewModel model)
        {
            if (model.SelectedFilamentIds == null || model.SelectedFilamentIds.Count == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedFilamentIds), "Select at least one filament.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await printService.CreatePrintAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await printService.GetEditViewModelAsync(id);

            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PrintCreateEditViewModel model)
        {
            if (model.SelectedFilamentIds == null || model.SelectedFilamentIds.Count == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedFilamentIds), "Select at least one filament.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await printService.EditPrintAsync(id, model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await printService.GetDeleteViewModelAsync(id);

            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await printService.DeletePrintAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
