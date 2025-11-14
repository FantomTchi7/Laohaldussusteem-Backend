using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ettevõtted",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aadress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registrikood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SwedbankKonto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SebKonto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ettevõtted", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kasutajad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KasutajaTüüp = table.Column<int>(type: "int", nullable: false),
                    Skype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kasutajad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arved",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoostatudKuupäev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Maksetähtaeg = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KoostajaId = table.Column<int>(type: "int", nullable: false),
                    TellijaId = table.Column<int>(type: "int", nullable: false),
                    EttevõteId = table.Column<int>(type: "int", nullable: false),
                    SummaKäibemaksuta = table.Column<int>(type: "int", nullable: false),
                    Käibemaksumäär = table.Column<int>(type: "int", nullable: false),
                    SummaKäibemaksuga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arved", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arved_Ettevõtted_EttevõteId",
                        column: x => x.EttevõteId,
                        principalTable: "Ettevõtted",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arved_Kasutajad_KoostajaId",
                        column: x => x.KoostajaId,
                        principalTable: "Kasutajad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arved_Kasutajad_TellijaId",
                        column: x => x.TellijaId,
                        principalTable: "Kasutajad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produktid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nimetus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ühik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaasHind = table.Column<int>(type: "int", nullable: false),
                    ProduktTüüp = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    ArveId = table.Column<int>(type: "int", nullable: true),
                    Kogus = table.Column<int>(type: "int", nullable: true),
                    Hind = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produktid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produktid_Arved_ArveId",
                        column: x => x.ArveId,
                        principalTable: "Arved",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arved_EttevõteId",
                table: "Arved",
                column: "EttevõteId");

            migrationBuilder.CreateIndex(
                name: "IX_Arved_KoostajaId",
                table: "Arved",
                column: "KoostajaId");

            migrationBuilder.CreateIndex(
                name: "IX_Arved_TellijaId",
                table: "Arved",
                column: "TellijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produktid_ArveId",
                table: "Produktid",
                column: "ArveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produktid");

            migrationBuilder.DropTable(
                name: "Arved");

            migrationBuilder.DropTable(
                name: "Ettevõtted");

            migrationBuilder.DropTable(
                name: "Kasutajad");
        }
    }
}
