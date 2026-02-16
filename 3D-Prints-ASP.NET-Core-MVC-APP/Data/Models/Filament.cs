using _3DPrintsAPP.Data.Enums;
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
        [ForeignKey(nameof(Printer))]
        public int PrinterId { get; set; }
        public virtual Printer Printer { get; set; } = null!;
        public virtual ICollection<PrintFilament> PrintFilaments { get; set; } 
            = new HashSet<PrintFilament>();
    }
}
