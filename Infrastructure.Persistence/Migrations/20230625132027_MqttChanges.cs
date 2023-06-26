using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MqttChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Sensors",
                newName: "MqttPath");

            migrationBuilder.AddColumn<string>(
                name: "ChipName",
                table: "Devices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "I2CAddress",
                table: "Devices",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "IntervalCount",
                table: "Devices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntervalType",
                table: "Devices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipName",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "I2CAddress",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IntervalCount",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IntervalType",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "MqttPath",
                table: "Sensors",
                newName: "Path");
        }
    }
}
