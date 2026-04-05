using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _3D_Prints_APP_Services.Contracts;
using _3DPrintsAPP.ViewModels;

namespace _3DPrintsAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPrintService printService;

        public HomeController(IPrintService printService)
        {
            this.printService = printService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ICollection<PrintViewModel> prints = await printService.GetLatestPublicPrintsAsync(9);
            return View(prints);
        }
    }
}