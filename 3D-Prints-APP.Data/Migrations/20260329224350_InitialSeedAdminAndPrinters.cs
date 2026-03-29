using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedAdminAndPrinters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Printers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id", 0, "admin-concurrency-stamp", "admin@site.com", true, false, null, "ADMIN@SITE.COM", "ADMIN@SITE.COM", null, null, false, "admin-security-stamp", false, "admin@site.com" });

            migrationBuilder.UpdateData(
                table: "Printers",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "admin-user-id");

            migrationBuilder.UpdateData(
                table: "Printers",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: "admin-user-id");

            migrationBuilder.CreateIndex(
                name: "IX_Printers_UserId",
                table: "Printers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Printers_AspNetUsers_UserId",
                table: "Printers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Printers_AspNetUsers_UserId",
                table: "Printers");

            migrationBuilder.DropIndex(
                name: "IX_Printers_UserId",
                table: "Printers");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Printers");
        }
    }
}
