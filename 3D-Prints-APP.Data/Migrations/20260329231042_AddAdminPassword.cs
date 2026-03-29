using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMgoypDgbqDzlnw4UlwgU6TYJT73IMHdtY2JV0a668gEYJDfxImXXZQSuODkFJHRWA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                column: "PasswordHash",
                value: null);
        }
    }
}
