using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class SeedPrints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Prints",
                columns: new[] { "Id", "Description", "IsPublic", "PrintTime", "PrinterId", "Title", "UploadPhoto", "UploadedTime", "UserId" },
                values: new object[,]
                {
                    { 1, "One of the minions", true, new TimeOnly(2, 30, 0), null, "Minion Bob", "https://m.media-amazon.com/images/I/61R3gavoPGL._AC_UF894,1000_QL80_.jpg", new DateTime(2025, 10, 10, 14, 30, 0, 0, DateTimeKind.Unspecified), "admin-user-id" },
                    { 2, "Popeye the sailor man", true, new TimeOnly(1, 45, 0), null, "Popeye", "https://i.ebayimg.com/images/g/ItYAAOSwM4Rmv4Sm/s-l400.jpg", new DateTime(2025, 10, 12, 16, 0, 0, 0, DateTimeKind.Unspecified), "admin-user-id" }
                });

            migrationBuilder.InsertData(
                table: "Prints",
                columns: new[] { "Id", "Description", "PrintTime", "PrinterId", "Title", "UploadPhoto", "UploadedTime", "UserId" },
                values: new object[,]
                {
                    { 3, "Holder that keeps cables organized", new TimeOnly(3, 0, 0), null, "Cable Holder", "https://www.3dforprint.com/modelos/10243/cable-holder1.webp", new DateTime(2025, 10, 15, 11, 20, 0, 0, DateTimeKind.Unspecified), "admin-user-id" },
                    { 4, "Organizers designed to securely hold tools like calipers, flush cutters, Allen keys, and scraper tools, keeping workbenches tidy", new TimeOnly(4, 15, 0), null, "Tool tray stand", "https://i.etsystatic.com/52313838/r/il/122c4d/6435355435/il_570xN.6435355435_cecf.jpg", new DateTime(2025, 10, 18, 18, 10, 0, 0, DateTimeKind.Unspecified), "admin-user-id" }
                });

            migrationBuilder.InsertData(
                table: "Prints",
                columns: new[] { "Id", "Description", "IsPublic", "PrintTime", "PrinterId", "Title", "UploadPhoto", "UploadedTime", "UserId" },
                values: new object[] { 6, "Famous 3D printer test model", true, new TimeOnly(1, 30, 0), null, "3DBenchy", "https://media.printables.com/media/prints/3161/images/20206_70fde6a0-6da1-4522-ba46-25f1bece7199/thumbs/cover/1200x630/jpg/benchy.jpg", new DateTime(2025, 10, 22, 9, 15, 0, 0, DateTimeKind.Unspecified), "admin-user-id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 6);
        }
    }
}
