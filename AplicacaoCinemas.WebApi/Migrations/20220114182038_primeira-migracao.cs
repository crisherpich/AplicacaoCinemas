using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AplicacaoCinemas.WebApi.Migrations
{
    public partial class primeiramigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filmes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(50)", nullable: true),
                    Sinopse = table.Column<string>(type: "varchar(max)", nullable: true),
                    Duracao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Dia = table.Column<DateTime>(type: "date", nullable: false),
                    HoraInicio = table.Column<int>(type: "int", nullable: false),
                    MinutoInicio = table.Column<int>(type: "int", nullable: false),
                    QuantidadeLugares = table.Column<int>(type: "int", nullable: false),
                    QuantidadeLugaresLivres = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<double>(type: "float", nullable: false),
                    IdFilme = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_concorrencia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessoes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filmes");

            migrationBuilder.DropTable(
                name: "Sessoes");
        }
    }
}
