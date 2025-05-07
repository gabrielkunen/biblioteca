using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class bcrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                schema: "biblioteca",
                table: "Funcionario");

            migrationBuilder.RenameColumn(
                name: "Salt",
                schema: "biblioteca",
                table: "Funcionario",
                newName: "Senha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Senha",
                schema: "biblioteca",
                table: "Funcionario",
                newName: "Salt");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                schema: "biblioteca",
                table: "Funcionario",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");
        }
    }
}
