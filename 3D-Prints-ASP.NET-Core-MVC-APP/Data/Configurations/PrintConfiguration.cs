using _3DPrintsAPP.Data.Enums;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintConfiguration : IEntityTypeConfiguration<Print>
    {
        public void Configure(EntityTypeBuilder<Print> builder)
        {
            builder.Property(p => p.UploadPhoto).HasMaxLength(2048);

            builder.HasData(
 
                new Print
                {
                    Id = 1,
                    Title = "Desk Organizer",
                    Description = "Organizer for tools, nozzles and spare parts.",
                    PrintTime = new TimeOnly(2, 15, 0),
                    UploadPhoto = "https://img1.yeggi.com/images-3d-model/86f/9869000_organizer-desktop-organizer-by-casperdesigns-makerworld-download-free-",
                    UploadedTime = new DateTime(2026, 2, 17),
                    PrinterId = 1
                },

                new Print
                {
                    Id = 2,
                    Title = "Wall Mount",
                    Description = "Simple mount for accessories and tools.",
                    PrintTime = new TimeOnly(1, 5, 0),
                    UploadPhoto = "https://fbi.cults3d.com/uploaders/16518072/illustration-file/34e80894-2287-4f98-ac0e-60b6e2086616/AR%20Mount%20v2%20v1.png",
                    UploadedTime = new DateTime(2026, 2, 17),
                    PrinterId = 2
                },

                new Print
                {
                    Id = 3,
                    Title = "Cable Holder",
                    Description = "Keeps cables organized on the desk.",
                    PrintTime = new TimeOnly(0, 45, 0),
                    UploadPhoto = "https://www.3dforprint.com/modelos/10243/cable-holder1.webp",
                    UploadedTime = new DateTime(2026, 2, 18),
                    PrinterId = 1
                },

                new Print
                {
                    Id = 4,
                    Title = "Phone Stand",
                    Description = "Compact phone stand for desk use.",
                    PrintTime = new TimeOnly(1, 20, 0),
                    UploadPhoto = "https://i.etsystatic.com/23796806/r/il/8180c7/5739653765/il_fullxfull.5739653765_is72.jpg",
                    UploadedTime = new DateTime(2026, 2, 18),
                    PrinterId = 2
                },

                new Print
                {
                    Id = 5,
                    Title = "Tool Tray",
                    Description = "Small tray for screws and tools.",
                    PrintTime = new TimeOnly(2, 50, 0),
                    UploadPhoto = "https://i.etsystatic.com/52313838/r/il/122c4d/6435355435/il_fullxfull.6435355435_cecf.jpg",
                    UploadedTime = new DateTime(2026, 2, 19),
                    PrinterId = 1
                }
            );
        }
    }
}
