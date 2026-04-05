namespace _3DPrintsAPP.ViewModels
{
    public class PrintViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public TimeOnly PrintTime { get; set; }

        public string UploadPhoto { get; set; } = null!;

        public DateTime UploadedTime { get; set; }

        public bool IsPublic { get; set; }

        public string? PrintOptionName { get; set; }

        public string? OwnerName { get; set; }
        public string? OwnerId { get; set; }
        public bool IsInCollection { get; set; }
    }
}