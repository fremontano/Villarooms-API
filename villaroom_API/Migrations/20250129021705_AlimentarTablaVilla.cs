using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace villaroom_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrado", "Name", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "Piscina", "Dettale Mercedes", new DateTime(2025, 1, 28, 23, 17, 4, 615, DateTimeKind.Local).AddTicks(6096), new DateTime(2025, 1, 28, 23, 17, 4, 613, DateTimeKind.Local).AddTicks(9995), "image_url", 120, "Villa Las Mercedes", 4, 200.0 },
                    { 2, "Gimnasio", "Dettale Oasis", new DateTime(2025, 1, 28, 23, 17, 4, 615, DateTimeKind.Local).AddTicks(6297), new DateTime(2025, 1, 28, 23, 17, 4, 615, DateTimeKind.Local).AddTicks(6292), "image_url", 150, "Villa El Oasis", 6, 250.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
