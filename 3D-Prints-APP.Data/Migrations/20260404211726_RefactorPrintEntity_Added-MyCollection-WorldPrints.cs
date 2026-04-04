using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _3DPrintsASP.NETCoreMVCAPP.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPrintEntity_AddedMyCollectionWorldPrints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prints_Printers_PrinterId",
                table: "Prints");

            migrationBuilder.DropTable(
                name: "PrintFilaments");

            migrationBuilder.AlterColumn<int>(
                name: "PrinterId",
                table: "Prints",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Prints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Prints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserCollectionPrints",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollectionPrints", x => new { x.UserId, x.PrintId });
                    table.ForeignKey(
                        name: "FK_UserCollectionPrints_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCollectionPrints_Prints_PrintId",
                        column: x => x.PrintId,
                        principalTable: "Prints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prints_UserId",
                table: "Prints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectionPrints_PrintId",
                table: "UserCollectionPrints",
                column: "PrintId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prints_AspNetUsers_UserId",
                table: "Prints",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prints_Printers_PrinterId",
                table: "Prints",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prints_AspNetUsers_UserId",
                table: "Prints");

            migrationBuilder.DropForeignKey(
                name: "FK_Prints_Printers_PrinterId",
                table: "Prints");

            migrationBuilder.DropTable(
                name: "UserCollectionPrints");

            migrationBuilder.DropIndex(
                name: "IX_Prints_UserId",
                table: "Prints");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Prints");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Prints");

            migrationBuilder.AlterColumn<int>(
                name: "PrinterId",
                table: "Prints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PrintFilaments",
                columns: table => new
                {
                    PrintId = table.Column<int>(type: "int", nullable: false),
                    FilamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintFilaments", x => new { x.PrintId, x.FilamentId });
                    table.ForeignKey(
                        name: "FK_PrintFilaments_Filaments_FilamentId",
                        column: x => x.FilamentId,
                        principalTable: "Filaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrintFilaments_Prints_PrintId",
                        column: x => x.PrintId,
                        principalTable: "Prints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_PrintFilaments_FilamentId",
                table: "PrintFilaments",
                column: "FilamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prints_Printers_PrinterId",
                table: "Prints",
                column: "PrinterId",
                principalTable: "Printers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
