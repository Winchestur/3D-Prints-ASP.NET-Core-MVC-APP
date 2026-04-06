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
        public string Description { get; set; } = null!;

        [Required]
        public TimeOnly PrintTime { get; set; }

        [Required]
        [StringLength(MaxImgUrl)]
        public string UploadPhoto { get; set; } = null!;

        [Required]
        public DateTime UploadedTime { get; set; }

        [Required]
        public bool IsPublic { get; set; } = false;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<UserCollectionPrint> UserCollectionPrints { get; set; }
            = new HashSet<UserCollectionPrint>();

        public virtual ICollection<PrintRating> Ratings { get; set; }
            = new HashSet<PrintRating>();
    }
}