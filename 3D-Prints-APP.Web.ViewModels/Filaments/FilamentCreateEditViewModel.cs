using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _3DPrintsAPP.ViewModels.Filaments
{
    public class FilamentCreateEditViewModel
    {
        [Required]
        public int FilamentOptionId { get; set; }

        public IEnumerable<SelectListItem> FilamentOptions { get; set; }
            = new List<SelectListItem>();
    }
}