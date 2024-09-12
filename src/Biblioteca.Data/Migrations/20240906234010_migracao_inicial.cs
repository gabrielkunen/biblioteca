using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Biblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracao_inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "biblioteca");

            migrationBuilder.CreateTable(
                name: "Autor",
                schema: "biblioteca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "varchar(500)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genero",
                schema: "biblioteca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "varchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                schema: "biblioteca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "varchar(500)", nullable: false),
                    Isbn = table.Column<string>(type: "varchar(13)", nullable: false),
                    Codigo = table.Column<string>(type: "varchar(30)", nullable: false),
                    IdAutor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Livro_Autor_IdAutor",
                        column: x => x.IdAutor,
                        principalSchema: "biblioteca",
                        principalTable: "Autor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneroLivro",
                schema: "biblioteca",
                columns: table => new
                {
                    GenerosId = table.Column<int>(type: "integer", nullable: false),
                    LivrosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroLivro", x => new { x.GenerosId, x.LivrosId });
                    table.ForeignKey(
                        name: "FK_GeneroLivro_Genero_GenerosId",
                        column: x => x.GenerosId,
                        principalSchema: "biblioteca",
                        principalTable: "Genero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneroLivro_Livro_LivrosId",
                        column: x => x.LivrosId,
                        principalSchema: "biblioteca",
                        principalTable: "Livro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneroLivro_LivrosId",
                schema: "biblioteca",
                table: "GeneroLivro",
                column: "LivrosId");

            migrationBuilder.CreateIndex(
                name: "IX_Livro_IdAutor",
                schema: "biblioteca",
                table: "Livro",
                column: "IdAutor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneroLivro",
                schema: "biblioteca");

            migrationBuilder.DropTable(
                name: "Genero",
                schema: "biblioteca");

            migrationBuilder.DropTable(
                name: "Livro",
                schema: "biblioteca");

            migrationBuilder.DropTable(
                name: "Autor",
                schema: "biblioteca");
        }
    }
}
