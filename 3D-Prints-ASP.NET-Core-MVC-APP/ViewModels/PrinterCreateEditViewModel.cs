using System.ComponentModel.DataAnnotations;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.ViewModels
{
    public class PrinterCreateEditViewModel
    {
        [Required]
        [StringLength(MaxModelNameLength, MinimumLength = MinModelNameLength)]
        public string ModelName { get; set; } = null!;

        [Required]
        public decimal NozzleDiameter { get; set; }

        [Required]
        [StringLength(MaxDescriptionLength, MinimumLength = MinDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(MaxImgUrl, MinimumLength = MinImgUrl)]
        public string UploadPhoto { get; set; } = null!;

        public bool AMS { get; set; }
    }
}
