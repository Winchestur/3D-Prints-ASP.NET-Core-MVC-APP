using _3DPrintsAPP.Data.Enums;

namespace _3DPrintsAPP.ViewModels
{
    public class FilamentViewModel
    {
        public int Id { get; set; }

        public Brand Brand { get; set; }
        public Materials Material { get; set; }
        public Colors FilamentColor { get; set; }
        public decimal Diameter { get; set; }
        public int PrinterId { get; set; }
        public string PrinterModelName { get; set; } = null!;
    }
}
