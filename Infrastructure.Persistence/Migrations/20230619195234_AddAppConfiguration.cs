using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MqttConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppConfigurationId = table.Column<int>(type: "integer", nullable: false),
                    IsClientEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    TcpServer = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MqttConfiguration_AppConfigurations_AppConfigurationId",
                        column: x => x.AppConfigurationId,
                        principalTable: "AppConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MqttConfiguration_AppConfigurationId",
                table: "MqttConfiguration",
                column: "AppConfigurationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MqttConfiguration");

            migrationBuilder.DropTable(
                name: "AppConfigurations");
        }
    }
}
