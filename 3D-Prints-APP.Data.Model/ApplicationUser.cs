
using Microsoft.AspNetCore.Identity;

namespace _3DPrintsAPP.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Print> Prints { get; set; }
    = new HashSet<Print>();

        public virtual ICollection<UserCollectionPrint> UserCollectionPrints { get; set; }
            = new HashSet<UserCollectionPrint>();
        public virtual ICollection<PrintRating> PrintRatings { get; set; }
            = new HashSet<PrintRating>();
    }
}