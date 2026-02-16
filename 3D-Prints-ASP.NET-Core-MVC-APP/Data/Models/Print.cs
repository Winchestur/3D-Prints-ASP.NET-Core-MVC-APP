using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static _3DPrintsAPP.Data.Validations.Validations;

namespace _3DPrintsAPP.Data.Models
{
    public class Print
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(MaxDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public TimeOnly PrintTime { get; set; }

        [Required]
        [StringLength(MaxImgUrl)]
        public string? UploadPhoto { get; set; }

        [Required]
        public DateTime UploadedTime { get; set; }

        [Required]
        [ForeignKey(nameof(Printer))]
        public int PrinterId { get; set; }
        public virtual Printer? Printer { get; set; }
        public virtual ICollection<PrintFilament> PrintFilaments { get; set; } = new HashSet<PrintFilament>();
    }
}
