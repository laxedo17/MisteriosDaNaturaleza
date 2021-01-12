using Microsoft.EntityFrameworkCore.Migrations;

namespace MisteriosDaNaturaleza.AccesoDatos.Migrations
{
    public partial class AddDestinoToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destino",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Precio = table.Column<double>(nullable: false),
                    DescLonga = table.Column<string>(nullable: true),
                    ImaxeUrl = table.Column<string>(nullable: true),
                    CategoriaId = table.Column<int>(nullable: false),
                    FrecuenciaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destino_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Destino_Frecuencia_FrecuenciaId",
                        column: x => x.FrecuenciaId,
                        principalTable: "Frecuencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destino_CategoriaId",
                table: "Destino",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Destino_FrecuenciaId",
                table: "Destino",
                column: "FrecuenciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destino");
        }
    }
}
