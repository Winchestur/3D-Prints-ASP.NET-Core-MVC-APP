using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFilamentToUserAndOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_Printers_PrinterId",
                table: "Filaments");

            migrationBuilder.AlterColumn<int>(
                name: "PrinterId",
                table: "Filaments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Filaments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Filaments_UserId",
                table: "Filaments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_AspNetUsers_UserId",
                table: "Filaments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments",
                column: "FilamentOptionId",
                principalTable: "FilamentOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_Printers_PrinterId",
                table: "Filaments",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_AspNetUsers_UserId",
                table: "Filaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments");

            migrationBuilder.DropForeignKey(
                name: "FK_Filaments_Printers_PrinterId",
                table: "Filaments");

            migrationBuilder.DropIndex(
                name: "IX_Filaments_UserId",
                table: "Filaments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Filaments");

            migrationBuilder.AlterColumn<int>(
                name: "PrinterId",
                table: "Filaments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_FilamentOptions_FilamentOptionId",
                table: "Filaments",
                column: "FilamentOptionId",
                principalTable: "FilamentOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filaments_Printers_PrinterId",
                table: "Filaments",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
