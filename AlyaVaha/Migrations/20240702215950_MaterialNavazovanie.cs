using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlyaVaha.Migrations
{
    /// <inheritdoc />
    public partial class MaterialNavazovanie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Navazovania",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_MaterialId",
                table: "Navazovania",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Navazovania_Materialy_MaterialId",
                table: "Navazovania",
                column: "MaterialId",
                principalTable: "Materialy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Navazovania_Materialy_MaterialId",
                table: "Navazovania");

            migrationBuilder.DropIndex(
                name: "IX_Navazovania_MaterialId",
                table: "Navazovania");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Navazovania");
        }
    }
}
