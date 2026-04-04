using _3DPrintsAPP.Enums;
using System.ComponentModel.DataAnnotations;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.Data.Models
{
    public class FilamentOption
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

        public virtual ICollection<Filament> Filaments { get; set; }
            = new HashSet<Filament>();
    }
}