using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "PrintFilaments");

            migrationBuilder.DropColumn(
                name: "UsedGrams",
                table: "PrintFilaments");

            migrationBuilder.InsertData(
                table: "Printers",
                columns: new[] { "Id", "AMS", "Description", "ModelName", "NozzleDiameter", "UploadPhoto", "UploadedTime" },
                values: new object[,]
                {
                    { 1, true, "Bambu Lab P1S with AMS for multi-color prints.", "Bambu Lab P1S", 1.75m, "https://store.bblcdn.com/s7/default/7abb7477d233498b82a02dc93dd069df/P1.jpg", new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, false, "Reliable budget printer for starters and upgrades.", "Creality Ender 3", 1.75m, "https://m.media-amazon.com/images/I/61CndtGd6wL._AC_UF350,350_QL80_.jpg", new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Filaments",
                columns: new[] { "Id", "Brand", "Diameter", "FilamentColor", "Material", "PrinterId", "UploadPhoto", "WeightKG" },
                values: new object[,]
                {
                    { 1, 11, 1.75m, 7, 1, 1, "https://cdn2.botland.com.pl/127588-large_default/filament-bambu-lab-pc-175mm-1kg-w-zestawie-z-wielorazowa-szpula-black.jpg", 1.0 },
                    { 2, 2, 1.75m, 8, 3, 2, "https://m.media-amazon.com/images/I/71eFciMUSaL._AC_UF1000,1000_QL80_.jpg", 1.0 },
                    { 3, 11, 1.75m, 3, 1, 1, "https://cdncloudcart.com/20502/products/images/467/bambu-lab-pla-cf-filament-s-karbonovi-vlakna-1-75mm-1kg-za-3d-printeri-65708d0c52b7f_150x150.jpeg?1744983052", 1.0 },
                    { 5, 3, 1.75m, 9, 1, 1, "https://www.prusa3d.com/content/images/product/3146.jpg", 0.75 },
                    { 6, 2, 1.75m, 3, 5, 1, "https://ruumik.ee/wp-content/uploads/2021/01/tpu_Translucent-Blue-3.jpg", 0.5 },
                    { 7, 11, 1.75m, 1, 2, 2, "https://botland.com.pl/img/art/inne/24646_2.jpg", 1.0 }
                });

            migrationBuilder.InsertData(
                table: "Prints",
                columns: new[] { "Id", "Description", "PrintTime", "PrinterId", "Title", "UploadPhoto", "UploadedTime" },
                values: new object[,]
                {
                    { 1, "Organizer for tools, nozzles and spare parts.", new TimeOnly(2, 15, 0), 1, "Desk Organizer", "https://img1.yeggi.com/images-3d-model/86f/9869000_organizer-desktop-organizer-by-casperdesigns-makerworld-download-free-", new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Simple mount for accessories and tools.", new TimeOnly(1, 5, 0), 2, "Wall Mount", "https://fbi.cults3d.com/uploaders/16518072/illustration-file/34e80894-2287-4f98-ac0e-60b6e2086616/AR%20Mount%20v2%20v1.png", new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Keeps cables organized on the desk.", new TimeOnly(0, 45, 0), 1, "Cable Holder", "https://www.3dforprint.com/modelos/10243/cable-holder1.webp", new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Compact phone stand for desk use.", new TimeOnly(1, 20, 0), 2, "Phone Stand", "https://i.etsystatic.com/23796806/r/il/8180c7/5739653765/il_fullxfull.5739653765_is72.jpg", new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Small tray for screws and tools.", new TimeOnly(2, 50, 0), 1, "Tool Tray", "https://i.etsystatic.com/52313838/r/il/122c4d/6435355435/il_fullxfull.6435355435_cecf.jpg", new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "PrintFilaments",
                columns: new[] { "FilamentId", "PrintId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 2, 4 },
                    { 5, 4 },
                    { 1, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "PrintFilaments",
                keyColumns: new[] { "FilamentId", "PrintId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Filaments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Prints",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prints",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prints",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prints",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prints",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Printers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Printers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "PrintFilaments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UsedGrams",
                table: "PrintFilaments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
