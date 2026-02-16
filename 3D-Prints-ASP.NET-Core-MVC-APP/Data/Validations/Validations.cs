namespace _3DPrintsAPP.Data.Validations
{
    public static class Validations
    {
        public const int MaxDescriptionLength = 1000;
        public const int MinDescriptionLength = 10;
        public const int MinImgUrl = 5;
        public const int MaxImgUrl = 2048;

        // Validation constants for Print model
        public const int MaxTitleLength = 100;
        public const int MinTitleLength = 3;

        // Validation constants for Printer model
        public const int MaxModelNameLength = 100;
        public const int MinModelNameLength = 3;
    
        // Validation constants for Filament model
        public const int MaxFilamentNameLength = 100;
        public const int MinFilamentNameLength = 3;
        public const double MaxWeight = 1.00;
        public const double MinWeight = 0.25;
    }
}
