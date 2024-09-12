using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Biblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class criar_emprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LimiteEmprestimo",
                schema: "biblioteca",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "biblioteca",
                table: "Livro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Emprestimo",
                schema: "biblioteca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdLivro = table.Column<int>(type: "integer", nullable: false),
                    IdFuncionario = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "date", nullable: false),
                    DataFim = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataDevolucao = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimo_Funcionario_IdFuncionario",
                        column: x => x.IdFuncionario,
                        principalSchema: "biblioteca",
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Emprestimo_Livro_IdLivro",
                        column: x => x.IdLivro,
                        principalSchema: "biblioteca",
                        principalTable: "Livro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Emprestimo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "biblioteca",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimo_IdFuncionario",
                schema: "biblioteca",
                table: "Emprestimo",
                column: "IdFuncionario");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimo_IdLivro",
                schema: "biblioteca",
                table: "Emprestimo",
                column: "IdLivro");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimo_IdUsuario",
                schema: "biblioteca",
                table: "Emprestimo",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emprestimo",
                schema: "biblioteca");

            migrationBuilder.DropColumn(
                name: "LimiteEmprestimo",
                schema: "biblioteca",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "biblioteca",
                table: "Livro");
        }
    }
}
