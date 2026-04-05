using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _3DPrintsAPP.Controllers
{
    [Authorize]
    public class MyCollectionController : Controller
    {
        private readonly IPrintService printService;
        private readonly UserManager<ApplicationUser> userManager;

        public MyCollectionController(
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

            var model = await printService.GetMyCollectionAsync(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            await printService.RemoveFromCollectionAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}