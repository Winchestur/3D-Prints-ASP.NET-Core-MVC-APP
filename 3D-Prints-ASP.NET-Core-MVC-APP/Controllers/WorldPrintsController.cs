using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class WorldPrintsController : Controller
    {
        private readonly IPrintService printService;
        private readonly UserManager<ApplicationUser> userManager;

        public WorldPrintsController(
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

            var model = await printService.GetWorldPrintsAsync(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCollection(int id)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            try
            {
                await printService.AddToCollectionAsync(id, userId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}