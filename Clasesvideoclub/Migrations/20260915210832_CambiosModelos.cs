using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clasesvideoclub.Migrations
{
    /// <inheritdoc />
    public partial class CambiosModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesAlquiler");

            migrationBuilder.DropColumn(
                name: "CantidadAlquilada",
                table: "Peliculas");

            migrationBuilder.DropColumn(
                name: "FechaDevolucionPrevista",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "MontoBase",
                table: "Alquileres");

            migrationBuilder.RenameColumn(
                name: "DNI",
                table: "Socios",
                newName: "Dni");

            migrationBuilder.RenameColumn(
                name: "MontoFinal",
                table: "Alquileres",
                newName: "MontoTotal");

            migrationBuilder.RenameColumn(
                name: "FechaDevolucionReal",
                table: "Alquileres",
                newName: "FechaDevolucion");

            migrationBuilder.AddColumn<int>(
                name: "DiasAlquilado",
                table: "Alquileres",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AlquilerDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlquilerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeliculaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlquilerDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlquilerDetalles_Alquileres_AlquilerId",
                        column: x => x.AlquilerId,
                        principalTable: "Alquileres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlquilerDetalles_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlquilerDetalles_AlquilerId",
                table: "AlquilerDetalles",
                column: "AlquilerId");

            migrationBuilder.CreateIndex(
                name: "IX_AlquilerDetalles_PeliculaId",
                table: "AlquilerDetalles",
                column: "PeliculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlquilerDetalles");

            migrationBuilder.DropColumn(
                name: "DiasAlquilado",
                table: "Alquileres");

            migrationBuilder.RenameColumn(
                name: "Dni",
                table: "Socios",
                newName: "DNI");

            migrationBuilder.RenameColumn(
                name: "MontoTotal",
                table: "Alquileres",
                newName: "MontoFinal");

            migrationBuilder.RenameColumn(
                name: "FechaDevolucion",
                table: "Alquileres",
                newName: "FechaDevolucionReal");

            migrationBuilder.AddColumn<int>(
                name: "CantidadAlquilada",
                table: "Peliculas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDevolucionPrevista",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "MontoBase",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DetallesAlquiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlquilerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeliculaId = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadDias = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecioPorDia = table.Column<decimal>(type: "TEXT", nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesAlquiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesAlquiler_Alquileres_AlquilerId",
                        column: x => x.AlquilerId,
                        principalTable: "Alquileres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesAlquiler_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAlquiler_AlquilerId",
                table: "DetallesAlquiler",
                column: "AlquilerId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesAlquiler_PeliculaId",
                table: "DetallesAlquiler",
                column: "PeliculaId");
        }
    }
}
