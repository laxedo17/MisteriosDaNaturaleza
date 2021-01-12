using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MisteriosDaNaturaleza.AccesoDatos.Migrations
{
    public partial class removeImaxeAsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Imaxe",
                table: "ImaxesWeb",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Imaxe",
                table: "ImaxesWeb",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
