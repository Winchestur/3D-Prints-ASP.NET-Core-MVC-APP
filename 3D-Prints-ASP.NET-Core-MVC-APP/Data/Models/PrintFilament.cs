using _3DPrintsAPP.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PrintFilament
{
    [Required]
    [ForeignKey(nameof(Print))]
    public int PrintId { get; set; }
    public Print Print { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Filament))]
    public int FilamentId { get; set; }
    public Filament Filament { get; set; } = null!;
    public decimal? UsedGrams { get; set; }
    public decimal Cost { get; set; }
}
