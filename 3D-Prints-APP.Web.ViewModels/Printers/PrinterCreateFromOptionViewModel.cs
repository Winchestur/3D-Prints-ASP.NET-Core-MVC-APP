using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _3DPrintsAPP.ViewModels.Printers
{
    public class PrinterCreateFromOptionViewModel
    {
        [Required]
        [Display(Name = "Printer Option")]
        public int PrinterOptionId { get; set; }

        public IEnumerable<SelectListItem> PrinterOptions { get; set; }
            = new List<SelectListItem>();
    }
}