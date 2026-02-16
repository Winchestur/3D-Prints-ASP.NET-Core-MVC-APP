namespace _3DPrintsAPP.Data.Validations
{
    public static class Validations
    {
        public const int MaxDescriptionLength = 1000;
        public const int MinDescriptionLength = 10;

        // Validation constants for Print model
        public const int MaxTitleLength = 100;
        public const int MinTitleLength = 3;

        // Validation constants for Printer model
        public const int MaxModelNameLength = 100;
        public const int MinModelNameLength = 3;
    
        // Validation constants for Filament model
        public const int MaxFilamentNameLength = 100;
        public const int MinFilamentNameLength = 3;
    }
}
