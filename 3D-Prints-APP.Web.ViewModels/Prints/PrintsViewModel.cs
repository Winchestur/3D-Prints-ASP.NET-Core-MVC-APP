using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.ViewModels
{
    public class PrintViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxTitleLength,
            MinimumLength = MinTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxDescriptionLength,
            MinimumLength = MinDescriptionLength)]
        public string Description { get; set; } = null!;

        [Required]
        public TimeOnly PrintTime { get; set; }

        [Required]
        [StringLength(MaxImgUrl,
            MinimumLength = MinImgUrl)]
        public string UploadPhoto { get; set; } = null!;

        public DateTime UploadedTime { get; set; }

        public int PrinterId { get; set; }

        [Required]
        [StringLength(MaxModelNameLength,
            MinimumLength = MinModelNameLength)]
        public string PrinterModelName { get; set; } = null!;

        public List<string> Filaments { get; set; } = new();
    }
}
