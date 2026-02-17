using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3DPrintsAPP.Data;
using _3DPrintsAPP.ViewModels;

namespace _3DPrintsAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _logger;

        public HomeController(ApplicationDbContext logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var prints = _logger.Prints
                .Include(p => p.Printer)
                .OrderByDescending(p => p.UploadedTime)
                .Take(9)
                .Select(p => new PrintViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description!,
                    PrintTime = p.PrintTime,
                    UploadPhoto = p.UploadPhoto!,
                    UploadedTime = p.UploadedTime,
                    PrinterId = p.PrinterId,
                    PrinterModelName = p.Printer!.ModelName!,
                    Filaments = new List<string>()
                })
                .ToList();

            return View(prints);
        }
    }
}
