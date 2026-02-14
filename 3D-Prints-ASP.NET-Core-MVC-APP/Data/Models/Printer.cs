using System.ComponentModel.DataAnnotations;
using static _3D_Prints_ASP.NET_Core_MVC_APP.Data.Validations.Validations;
using _3D_Prints_ASP.NET_Core_MVC_APP.Data.Enums;

namespace _3D_Prints_ASP.NET_Core_MVC_APP.Data.Models
{
    public class Printer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxModelNameLength)]
        public string? ModelName { get; set; }

        [Required]
        public decimal NozzleDiameter { get; set; }

        [Required]
        [StringLength(MaxDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public string? UploadPhoto { get; set; }

        [Required]
        public bool AMS { get; set; }
        public DateTime UploadedTime { get; set; }
        
        public virtual ICollection<Print> Prints { get; set; } 
            = new HashSet<Print>();
        
        public virtual ICollection<Filament> Filaments { get; set; } 
            = new HashSet<Filament>();
    }
}
