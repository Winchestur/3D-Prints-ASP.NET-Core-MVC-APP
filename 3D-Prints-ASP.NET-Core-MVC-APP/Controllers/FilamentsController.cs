using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.ViewModels.Filaments;
using Microsoft.AspNetCore.Mvc;

namespace _3DPrintsAPP.Controllers
{
    public class FilamentsController : Controller
    {
        private readonly IFilamentService filamentService;

        public FilamentsController(IFilamentService filamentService)
        {
            this.filamentService = filamentService;
        }

        public async Task<IActionResult> Index()
        {
            var filaments = await filamentService.GetAllFilamentsAsync();
            return View(filaments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var filament = await filamentService.GetFilamentDetailsAsync(id);
            if (filament == null) return NotFound();
            return View(filament);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await filamentService.GetCreateViewModelAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilamentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await filamentService.GetCreateViewModelAsync());

            await filamentService.CreateFilamentAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await filamentService.GetEditViewModelAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FilamentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await filamentService.GetEditViewModelAsync(id));

            await filamentService.EditFilamentAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var filament = await filamentService.GetFilamentDetailsAsync(id);
            if (filament == null) return NotFound();
            return View(filament);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await filamentService.DeleteFilamentAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
