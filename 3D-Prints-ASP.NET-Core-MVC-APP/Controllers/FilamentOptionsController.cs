using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class FilamentOptionsController : Controller
    {
        private readonly IFilamentOptionService filamentOptionService;

        public FilamentOptionsController(IFilamentOptionService filamentOptionService)
        {
            this.filamentOptionService = filamentOptionService;
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

            var options = await filamentOptionService.GetAllAsync();
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
        public async Task<IActionResult> Create(FilamentOption model)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await filamentOptionService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            var option = await filamentOptionService.GetByIdAsync(id);

            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FilamentOption model)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = await filamentOptionService.UpdateAsync(id, model);

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

            var option = await filamentOptionService.GetByIdAsync(id);

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

            bool isDeleted = await filamentOptionService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}