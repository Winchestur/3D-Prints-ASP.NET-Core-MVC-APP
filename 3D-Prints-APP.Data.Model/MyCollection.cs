using System.ComponentModel.DataAnnotations.Schema;

namespace _3DPrintsAPP.Data.Models
{
    public class UserCollectionPrint
    {
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        public int PrintId { get; set; }
        [ForeignKey(nameof(PrintId))]
        public virtual Print Print { get; set; } = null!;
    }
}