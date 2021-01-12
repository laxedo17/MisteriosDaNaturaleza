using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MisteriosDaNaturaleza.AccesoDatos.Migrations
{
    public partial class addCabeceiraPedidoAndDetallesPedidoToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CabeceiraPedido",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Telefono = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Direccion = table.Column<string>(nullable: false),
                    Poblacion = table.Column<string>(nullable: false),
                    CP = table.Column<string>(nullable: false),
                    DataPedido = table.Column<DateTime>(nullable: false),
                    Estado = table.Column<string>(nullable: true),
                    Comentarios = table.Column<string>(nullable: true),
                    ContaDestinos = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabeceiraPedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedido",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCabeceiraPedido = table.Column<int>(nullable: false),
                    DestinoId = table.Column<int>(nullable: false),
                    IdDestino = table.Column<int>(nullable: true),
                    NomeServicio = table.Column<string>(nullable: false),
                    Precio = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_CabeceiraPedido_IdCabeceiraPedido",
                        column: x => x.IdCabeceiraPedido,
                        principalTable: "CabeceiraPedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Destino_IdDestino",
                        column: x => x.IdDestino,
                        principalTable: "Destino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_IdCabeceiraPedido",
                table: "DetallesPedido",
                column: "IdCabeceiraPedido");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_IdDestino",
                table: "DetallesPedido",
                column: "IdDestino");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "CabeceiraPedido");
        }
    }
}
