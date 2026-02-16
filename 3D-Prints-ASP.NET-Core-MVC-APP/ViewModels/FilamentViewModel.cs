using _3DPrintsAPP.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.ViewModels
{
    public class FilamentViewModel
    {
        [Key]
        public int Id { get; set; }

        public Brand Brand { get; set; }
        public Materials Material { get; set; }
        public Colors FilamentColor { get; set; }

        [Required]
        [StringLength(MaxImgUrl, MinimumLength = MinImgUrl)]
        public string UploadPhoto { get; set; } = null!;
        
        [Required]
        [Range(MinWeight, MaxWeight)]
        public double WeightKg { get; set; }
        public decimal Diameter { get; set; }
        public int PrinterId { get; set; }
        public string PrinterModelName { get; set; } = null!;
    }
}
