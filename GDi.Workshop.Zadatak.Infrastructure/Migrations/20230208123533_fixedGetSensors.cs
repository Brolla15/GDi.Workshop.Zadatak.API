using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GDi.Workshop.Zadatak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixedGetSensors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SensorTypeId",
                table: "Sensors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SensorTypeId",
                table: "Sensors",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
