using _3DPrintsAPP.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.Data.Models
{
    public class Filament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Brand Brand { get; set; }

        [Required]
        public Materials Material { get; set; }

        [Required]
        public Colors FilamentColor { get; set; }

        [Required]
        [StringLength(MaxImgUrl)]
        public string? UploadPhoto { get; set; }

        [Required]
        [Range(MinWeight, MaxWeight)]
        public double WeightKG { get; set; }

        [Required]
        public decimal Diameter { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int FilamentOptionId { get; set; }
        public virtual FilamentOption FilamentOption { get; set; } = null!;

    }
}
