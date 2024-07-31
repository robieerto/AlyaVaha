using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AlyaVaha.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materialy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazovMaterialu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumVytvorenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumUpravy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HmotnostMaterialu = table.Column<double>(type: "float", nullable: true),
                    JeAktivny = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materialy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzivatelia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heslo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JeAdmin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzivatelia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zariadenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazovZariadenia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAdresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: true),
                    PocetNavazeni = table.Column<int>(type: "int", nullable: false),
                    NavazeneMnozstvo = table.Column<double>(type: "float", nullable: false),
                    NavazenyPocetDavok = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zariadenia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zasobniky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazovZasobnika = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumVytvorenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumUpravy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Skratka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CestaDoVahy = table.Column<bool>(type: "bit", nullable: true),
                    CestaZVahy = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zasobniky", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Navazovania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumStartu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CasStartu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumKonca = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CasKonca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZariadenieId = table.Column<int>(type: "int", nullable: true),
                    NavazeneMnozstvo = table.Column<double>(type: "float", nullable: true),
                    NavazenyPocetDavok = table.Column<int>(type: "int", nullable: true),
                    PozadovaneMnozstvo = table.Column<double>(type: "float", nullable: true),
                    PozadovanyPocetDavok = table.Column<int>(type: "int", nullable: true),
                    VelkostDavky = table.Column<double>(type: "float", nullable: true),
                    OdkialId = table.Column<int>(type: "int", nullable: true),
                    KamId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    UzivatelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navazovania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navazovania_Materialy_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materialy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navazovania_Uzivatelia_UzivatelId",
                        column: x => x.UzivatelId,
                        principalTable: "Uzivatelia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navazovania_Zariadenia_ZariadenieId",
                        column: x => x.ZariadenieId,
                        principalTable: "Zariadenia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navazovania_Zasobniky_KamId",
                        column: x => x.KamId,
                        principalTable: "Zasobniky",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Navazovania_Zasobniky_OdkialId",
                        column: x => x.OdkialId,
                        principalTable: "Zasobniky",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Materialy",
                columns: new[] { "Id", "DatumUpravy", "DatumVytvorenia", "HmotnostMaterialu", "JeAktivny", "NazovMaterialu" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9949), new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9874), 100.0, true, "Materiál 1" },
                    { 2, new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9957), new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9956), 200.0, true, "Materiál 2" }
                });

            migrationBuilder.InsertData(
                table: "Uzivatelia",
                columns: new[] { "Id", "Email", "Heslo", "JeAdmin", "Login" },
                values: new object[,]
                {
                    { 1, null, "alya123456", true, "admin" },
                    { 2, null, "obsluha123", false, "obsluha" }
                });

            migrationBuilder.InsertData(
                table: "Zariadenia",
                columns: new[] { "Id", "IpAdresa", "NavazeneMnozstvo", "NavazenyPocetDavok", "NazovZariadenia", "PocetNavazeni", "Port" },
                values: new object[] { 1, "192.168.1.10", 0.0, 0, "Váha 1", 0, 3396 });

            migrationBuilder.InsertData(
                table: "Zasobniky",
                columns: new[] { "Id", "CestaDoVahy", "CestaZVahy", "DatumUpravy", "DatumVytvorenia", "NazovZasobnika", "Skratka" },
                values: new object[,]
                {
                    { 1, true, true, new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9965), new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9963), "Zásobník 1", "Z1" },
                    { 2, true, true, new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9971), new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9970), "Zásobník 2", "Z2" },
                    { 3, true, true, new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9974), new DateTime(2024, 7, 31, 9, 6, 14, 798, DateTimeKind.Local).AddTicks(9973), "Zásobník 3", "Z3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_KamId",
                table: "Navazovania",
                column: "KamId");

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_MaterialId",
                table: "Navazovania",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_OdkialId",
                table: "Navazovania",
                column: "OdkialId");

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_UzivatelId",
                table: "Navazovania",
                column: "UzivatelId");

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_ZariadenieId",
                table: "Navazovania",
                column: "ZariadenieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navazovania");

            migrationBuilder.DropTable(
                name: "Materialy");

            migrationBuilder.DropTable(
                name: "Uzivatelia");

            migrationBuilder.DropTable(
                name: "Zariadenia");

            migrationBuilder.DropTable(
                name: "Zasobniky");
        }
    }
}
