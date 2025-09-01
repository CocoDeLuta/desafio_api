using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoTransacao",
                table: "Transacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoTransacao",
                table: "Transacoes");
        }
    }
}
