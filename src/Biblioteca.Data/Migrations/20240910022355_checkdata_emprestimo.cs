using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class checkdata_emprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_DataFim_MaiorQue_DataInicio",
                schema: "biblioteca",
                table: "Emprestimo",
                sql: "\"DataFim\" > \"DataInicio\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DataFim_MaiorQue_DataInicio",
                schema: "biblioteca",
                table: "Emprestimo");
        }
    }
}
