using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using _3DPrintsAPP.Data.Enums;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.ViewModels
{
    public class FilamentCreateEditViewModel
    {
        [Required]
        public Brand Brand { get; set; }

        [Required]
        public Materials Material { get; set; }

        [Required]
        public Colors FilamentColor { get; set; }

        [Required]
        [StringLength(MaxImgUrl, MinimumLength = MinImgUrl)]
        public string UploadPhoto { get; set; } = null!;

        [Required]
        [Range(MinWeight, MaxWeight)]
        public double WeightKg { get; set; }

        [Required]
        public decimal Diameter { get; set; }

        [Required]
        public int PrinterId { get; set; }

        // Dropdown lists
        public IEnumerable<SelectListItem> BrandOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> MaterialOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ColorOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> PrinterOptions { get; set; } = new List<SelectListItem>();
    }
}
