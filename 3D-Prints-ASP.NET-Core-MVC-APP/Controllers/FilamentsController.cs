using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.ViewModels.Filaments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class FilamentsController : Controller
    {
        private readonly IFilamentService filamentService;

        public FilamentsController(IFilamentService filamentService)
        {
            this.filamentService = filamentService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        public async Task<IActionResult> Index()
        {
            var filaments = await filamentService.GetAllFilamentsAsync(GetUserId());
            return View(filaments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var filament = await filamentService.GetFilamentDetailsAsync(id, GetUserId());
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

            await filamentService.CreateFilamentAsync(model, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await filamentService.GetEditViewModelAsync(id, GetUserId());
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FilamentCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await filamentService.GetEditViewModelAsync(id, GetUserId()));

            await filamentService.EditFilamentAsync(id, model, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var filament = await filamentService.GetFilamentDetailsAsync(id, GetUserId());
            if (filament == null) return NotFound();
            return View(filament);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await filamentService.DeleteFilamentAsync(id, GetUserId());
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}