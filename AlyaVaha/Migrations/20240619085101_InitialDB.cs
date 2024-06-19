using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlyaVaha.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
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
                    HmotnostMaterialu = table.Column<double>(type: "float", nullable: true)
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
                    Port = table.Column<int>(type: "int", nullable: true)
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
                    Skratka = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zasobniky", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cesty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumVytvorenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumUpravy = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZariadenieId = table.Column<int>(type: "int", nullable: true),
                    ZasobnikId = table.Column<int>(type: "int", nullable: true),
                    DoVahy = table.Column<bool>(type: "bit", nullable: true),
                    ZVahy = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cesty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cesty_Zariadenia_ZariadenieId",
                        column: x => x.ZariadenieId,
                        principalTable: "Zariadenia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cesty_Zasobniky_ZasobnikId",
                        column: x => x.ZasobnikId,
                        principalTable: "Zasobniky",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Navazovania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasStartu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CasKonca = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZariadenieId = table.Column<int>(type: "int", nullable: true),
                    NavazeneMnozstvo = table.Column<double>(type: "float", nullable: true),
                    NavazenyPocetDavok = table.Column<int>(type: "int", nullable: true),
                    PozadovaneMnozstvo = table.Column<double>(type: "float", nullable: true),
                    PozadovanyPocetDavok = table.Column<int>(type: "int", nullable: true),
                    VelkostDavky = table.Column<double>(type: "float", nullable: true),
                    OdkialId = table.Column<int>(type: "int", nullable: true),
                    KamId = table.Column<int>(type: "int", nullable: true),
                    UzivatelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navazovania", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Cesty_ZariadenieId",
                table: "Cesty",
                column: "ZariadenieId");

            migrationBuilder.CreateIndex(
                name: "IX_Cesty_ZasobnikId",
                table: "Cesty",
                column: "ZasobnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Navazovania_KamId",
                table: "Navazovania",
                column: "KamId");

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
                name: "Cesty");

            migrationBuilder.DropTable(
                name: "Materialy");

            migrationBuilder.DropTable(
                name: "Navazovania");

            migrationBuilder.DropTable(
                name: "Uzivatelia");

            migrationBuilder.DropTable(
                name: "Zariadenia");

            migrationBuilder.DropTable(
                name: "Zasobniky");
        }
    }
}
