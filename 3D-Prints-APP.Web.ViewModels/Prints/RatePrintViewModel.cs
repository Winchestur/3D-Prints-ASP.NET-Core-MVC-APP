using System.ComponentModel.DataAnnotations;

namespace _3DPrintsAPP.ViewModels
{
    public class RatePrintViewModel
    {
        public int PrintId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
    }
}