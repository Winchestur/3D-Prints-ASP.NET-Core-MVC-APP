using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _3DPrintsAPP.Data.Models
{
    public class PrintRating
    {
        [Required]
        public int PrintId { get; set; }

        [ForeignKey(nameof(PrintId))]
        public virtual Print Print { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Range(1, 5)]
        public int Value { get; set; }
    }
}