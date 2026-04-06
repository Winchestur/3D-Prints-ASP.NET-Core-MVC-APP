using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class PrintConfiguration : IEntityTypeConfiguration<Print>
    {
        public void Configure(EntityTypeBuilder<Print> builder)
        {
            builder.Property(p => p.UploadPhoto)
                .HasMaxLength(2048);

            builder.Property(p => p.IsPublic)
                .HasDefaultValue(false);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Prints)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(GetSeedPrints());
        }

        private List<Print> GetSeedPrints()
        {
            return new List<Print>
            {
                new Print
                {
                    Id = 1,
                    Title = "Minion Bob",
                    Description = "One of the minions",
                    PrintTime = new TimeOnly(2, 30),
                    UploadPhoto = "https://m.media-amazon.com/images/I/61R3gavoPGL._AC_UF894,1000_QL80_.jpg",
                    UploadedTime = new DateTime(2025, 10, 10, 14, 30, 0),
                    IsPublic = true,
                    UserId = "admin-user-id"
                },
                new Print
                {
                    Id = 2,
                    Title = "Popeye",
                    Description = "Popeye the sailor man",
                    PrintTime = new TimeOnly(1, 45),
                    UploadPhoto = "https://i.ebayimg.com/images/g/ItYAAOSwM4Rmv4Sm/s-l400.jpg",
                    UploadedTime = new DateTime(2025, 10, 12, 16, 0, 0),
                    IsPublic = true,
                    UserId = "admin-user-id"
                },
                new Print
                {
                    Id = 3,
                    Title = "Cable Holder",
                    Description = "Holder that keeps cables organized",
                    PrintTime = new TimeOnly(3, 0),
                    UploadPhoto = "https://www.3dforprint.com/modelos/10243/cable-holder1.webp",
                    UploadedTime = new DateTime(2025, 10, 15, 11, 20, 0),
                    IsPublic = false,
                    UserId = "admin-user-id"
                },
                new Print
                {
                    Id = 4,
                    Title = "Tool tray stand",
                    Description = "Organizers designed to securely hold tools like calipers, flush cutters, Allen keys, and scraper tools, keeping workbenches tidy",
                    PrintTime = new TimeOnly(4, 15),
                    UploadPhoto = "https://i.etsystatic.com/52313838/r/il/122c4d/6435355435/il_570xN.6435355435_cecf.jpg",
                    UploadedTime = new DateTime(2025, 10, 18, 18, 10, 0),
                    IsPublic = false,
                    UserId = "admin-user-id"
                },
                new Print
                {
                    Id = 6,
                    Title = "3DBenchy",
                    Description = "Famous 3D printer test model",
                    PrintTime = new TimeOnly(1, 30),
                    UploadPhoto = "https://media.printables.com/media/prints/3161/images/20206_70fde6a0-6da1-4522-ba46-25f1bece7199/thumbs/cover/1200x630/jpg/benchy.jpg",
                    UploadedTime = new DateTime(2025, 10, 22, 9, 15, 0),
                    IsPublic = true,
                    UserId = "admin-user-id"
                }
            };
        }
    }
}