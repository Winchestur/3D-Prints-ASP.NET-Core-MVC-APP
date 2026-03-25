using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.ViewModels
{
    public class PrintCreateEditViewModel
    {
        [Required]
        [StringLength(MaxTitleLength,
            MinimumLength = MinTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxDescriptionLength,
            MinimumLength = MinDescriptionLength)]
        public string Description { get; set; } = null!;

        public TimeOnly PrintTime { get; set; }

        [Required]
        [StringLength(MaxImgUrl,
            MinimumLength = MinImgUrl)]
        public string UploadPhoto { get; set; } = null!;

        public int PrinterId { get; set; }

        public List<int> SelectedFilamentIds { get; set; } = new();

        public List<SelectListItem> PrinterOptions { get; set; } = new();

        public List<SelectListItem> FilamentOptions { get; set; } = new();
    }
}
