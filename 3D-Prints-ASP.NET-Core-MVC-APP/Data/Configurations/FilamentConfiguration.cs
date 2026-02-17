using _3DPrintsAPP.Data.Enums;
using _3DPrintsAPP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3DPrintsAPP.Data.Configurations
{
    public class FilamentConfiguration : IEntityTypeConfiguration<Filament>
    {
        public void Configure(EntityTypeBuilder<Filament> builder)
        {
            builder.HasData(
                new Filament
                {
                    Id = 1,
                    Brand = Brand.BambuLab,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Black,
                    Diameter = 1.75m,
                    PrinterId = 1,
                    UploadPhoto = "https://cdn2.botland.com.pl/127588-large_default/filament-bambu-lab-pc-175mm-1kg-w-zestawie-z-wielorazowa-szpula-black.jpg",
                    WeightKG = 1.0
                },
                new Filament
                {
                    Id = 2,
                    Brand = Brand.eSUN,
                    Material = Materials.PETG,
                    FilamentColor = Colors.White,
                    Diameter = 1.75m,
                    PrinterId = 2,
                    UploadPhoto = "https://m.media-amazon.com/images/I/71eFciMUSaL._AC_UF1000,1000_QL80_.jpg",
                    WeightKG = 1.0
                },

                new Filament
                {
                    Id = 3,
                    Brand = Brand.BambuLab,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Blue,
                    Diameter = 1.75m,
                    PrinterId = 1,
                    UploadPhoto = "https://cdncloudcart.com/20502/products/images/467/bambu-lab-pla-cf-filament-s-karbonovi-vlakna-1-75mm-1kg-za-3d-printeri-65708d0c52b7f_150x150.jpeg?1744983052",
                    WeightKG = 1.0
                },
                new Filament
                {
                    Id = 5,
                    Brand = Brand.Prusament,
                    Material = Materials.PLA,
                    FilamentColor = Colors.Gray,
                    Diameter = 1.75m,
                    PrinterId = 1,
                    UploadPhoto = "https://www.prusa3d.com/content/images/product/3146.jpg",
                    WeightKG = 0.75
                },

                new Filament
                {
                    Id = 6,
                    Brand = Brand.eSUN,
                    Material = Materials.TPU,
                    FilamentColor = Colors.Blue,
                    Diameter = 1.75m,
                    PrinterId = 1,
                    UploadPhoto = "https://ruumik.ee/wp-content/uploads/2021/01/tpu_Translucent-Blue-3.jpg",
                    WeightKG = 0.5
                },

                new Filament
                {
                    Id = 7,
                    Brand = Brand.BambuLab,
                    Material = Materials.ABS,
                    FilamentColor = Colors.Red,
                    Diameter = 1.75m,
                    PrinterId = 2,
                    UploadPhoto = "https://botland.com.pl/img/art/inne/24646_2.jpg",
                    WeightKG = 1.0
                }
            );
        }
    }
}
