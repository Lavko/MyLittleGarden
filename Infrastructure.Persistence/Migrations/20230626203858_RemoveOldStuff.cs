using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakenActions");

            migrationBuilder.DropTable(
                name: "ActionRules");

            migrationBuilder.DropTable(
                name: "Measures");

            migrationBuilder.DropTable(
                name: "OutletConfigurations");

            migrationBuilder.DropTable(
                name: "ScheduledTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroundHumidity = table.Column<double>(type: "double precision", nullable: false),
                    Humidity = table.Column<double>(type: "double precision", nullable: false),
                    Pressure = table.Column<double>(type: "double precision", nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutletConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    OutletId = table.Column<int>(type: "integer", nullable: false),
                    PinId = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hour = table.Column<int>(type: "integer", nullable: false),
                    Minute = table.Column<int>(type: "integer", nullable: false),
                    Second = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OutletConfigurationId = table.Column<int>(type: "integer", nullable: false),
                    ScheduledTimeId = table.Column<int>(type: "integer", nullable: true),
                    ComparisonType = table.Column<int>(type: "integer", nullable: false),
                    ComparisonValue = table.Column<string>(type: "text", nullable: false),
                    IsSchedule = table.Column<bool>(type: "boolean", nullable: false),
                    MeasureProperty = table.Column<string>(type: "text", nullable: false),
                    OutletAction = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionRules_OutletConfigurations_OutletConfigurationId",
                        column: x => x.OutletConfigurationId,
                        principalTable: "OutletConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionRules_ScheduledTime_ScheduledTimeId",
                        column: x => x.ScheduledTimeId,
                        principalTable: "ScheduledTime",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TakenActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionRuleId = table.Column<int>(type: "integer", nullable: false),
                    EnvironmentMeasureId = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakenActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakenActions_ActionRules_ActionRuleId",
                        column: x => x.ActionRuleId,
                        principalTable: "ActionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakenActions_Measures_EnvironmentMeasureId",
                        column: x => x.EnvironmentMeasureId,
                        principalTable: "Measures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionRules_OutletConfigurationId",
                table: "ActionRules",
                column: "OutletConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionRules_ScheduledTimeId",
                table: "ActionRules",
                column: "ScheduledTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutletConfigurations_OutletId",
                table: "OutletConfigurations",
                column: "OutletId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TakenActions_ActionRuleId",
                table: "TakenActions",
                column: "ActionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TakenActions_EnvironmentMeasureId",
                table: "TakenActions",
                column: "EnvironmentMeasureId");
        }
    }
}
