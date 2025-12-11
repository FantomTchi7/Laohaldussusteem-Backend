using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LaduId",
                table: "Produktid",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Laod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aadress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produktid_LaduId",
                table: "Produktid",
                column: "LaduId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produktid_Laod_LaduId",
                table: "Produktid",
                column: "LaduId",
                principalTable: "Laod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produktid_Laod_LaduId",
                table: "Produktid");

            migrationBuilder.DropTable(
                name: "Laod");

            migrationBuilder.DropIndex(
                name: "IX_Produktid_LaduId",
                table: "Produktid");

            migrationBuilder.DropColumn(
                name: "LaduId",
                table: "Produktid");
        }
    }
}
