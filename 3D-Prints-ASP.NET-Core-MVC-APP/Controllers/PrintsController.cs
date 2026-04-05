using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using _3DPrintsAPP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class PrintsController : Controller
    {
        private readonly IPrintService printService;
        private readonly UserManager<ApplicationUser> userManager;

        public PrintsController(
            IPrintService printService,
            UserManager<ApplicationUser> userManager)
        {
            this.printService = printService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ICollection<PrintViewModel> prints = await printService.GetAllPrintsAsync(userId);
            return View(prints);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            PrintViewModel? viewModel = await printService.GetPrintDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            PrintCreateEditViewModel viewModel = await printService.GetCreateViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrintCreateEditViewModel model)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model = await printService.RebuildCreateEditViewModelAsync(model);
                return View(model);
            }

            await printService.CreatePrintAsync(model, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            PrintCreateEditViewModel? viewModel = await printService.GetEditViewModelAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrintCreateEditViewModel model)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model = await printService.RebuildCreateEditViewModelAsync(model);
                return View(model);
            }

            try
            {
                await printService.EditPrintAsync(id, model, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            PrintViewModel? viewModel = await printService.GetDeleteViewModelAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            try
            {
                await printService.DeletePrintAsync(id, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishToWorld(int id)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            try
            {
                await printService.PublishToWorldAsync(id, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePrivate(int id)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            try
            {
                await printService.MakePrivateAsync(id, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}