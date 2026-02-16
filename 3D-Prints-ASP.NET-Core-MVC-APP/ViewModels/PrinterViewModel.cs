namespace _3DPrintsAPP.ViewModels
{
    public class PrinterViewModel
    {
        public int Id { get; set; }

        public string ModelName { get; set; } = null!;

        public decimal NozzleDiameter { get; set; }

        public string Description { get; set; } = null!;

        public string UploadPhoto { get; set; } = null!;

        public bool AMS { get; set; }

        public DateTime UploadedTime { get; set; }
    }
}
