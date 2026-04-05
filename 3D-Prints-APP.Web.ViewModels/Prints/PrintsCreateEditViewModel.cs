using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _3DPrintsAPP.ViewModels
{
    public class PrintCreateEditViewModel
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public TimeOnly PrintTime { get; set; }

        [Required]
        public string UploadPhoto { get; set; } = null!;

        public int? PrintOptionId { get; set; }

        public ICollection<SelectListItem> PrintOptions { get; set; }
            = new List<SelectListItem>();
    }
}