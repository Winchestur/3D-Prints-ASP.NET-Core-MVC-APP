using System.ComponentModel.DataAnnotations;

namespace _3DPrintsAPP.Data.Models
{
    public class PrinterOption
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ModelName { get; set; } = null!;

        [Required]
        public decimal NozzleDiameter { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(2048)]
        public string UploadPhoto { get; set; } = null!;

        [Required]
        public bool AMS { get; set; }

        [Required]
        public DateTime UploadedTime { get; set; }

        public ICollection<Printer> Printers { get; set; } = new HashSet<Printer>();
    }
}