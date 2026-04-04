using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddFilamentOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilamentOptionId",
                table: "Filaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FilamentOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<int>(type: "int", nullable: false),
                    Material = table.Column<int>(type: "int", nullable: false),
                    FilamentColor = table.Column<int>(type: "int", nullable: false),
                    UploadPhoto = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    WeightKG = table.Column<double>(type: "float", nullable: false),
                    Diameter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FilamentOptions",
                columns: new[] { "Id", "Brand", "Diameter", "FilamentColor", "Material", "UploadPhoto", "WeightKG" },
                values: new object[,]
                {
                    { 1, 11, 1.75m, 7, 1, "https://cdn2.botland.com.pl/127588-large_default/filament-bambu-lab-pc-175mm-1kg-w-zestawie-z-wielorazowa-szpula-black.jpg", 1.0 },
                    { 2, 2, 1.75m, 8, 3, "https://m.media-amazon.com/images/I/71eFciMUSaL._AC_UF1000,1000_QL80_.jpg", 1.0 },
                    { 3, 11, 1.75m, 3, 1, "https://cdncloudcart.com/20502/products/images/467/bambu-lab-pla-cf-filament-s-karbonovi-vlakna-1-75mm-1kg-za-3d-printeri-65708d0c52b7f_150x150.jpeg?1744983052", 1.0 },
                    { 5, 3, 1.75m, 9, 1, "https://www.prusa3d.com/content/images/product/3146.jpg", 0.75 },
                    { 6, 2, 1.75m, 3, 5, "https://ruumik.ee/wp-content/uploads/2021/01/tpu_Translucent-Blue-3.jpg", 0.5 },
                    { 7, 11, 1.75m, 1, 2, "https://botland.com.pl/img/art/inne/24646_2.jpg", 1.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filaments_FilamentOptionId",
                table: "Filaments",
                column: "FilamentOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments",
                column: "FilamentOptionId",
                principalTable: "FilamentOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments");

            migrationBuilder.DropTable(
                name: "FilamentOptions");

            migrationBuilder.DropIndex(
                name: "IX_Filaments_FilamentOptionId",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "FilamentOptionId",
                table: "Filaments");
        }
    }
}
