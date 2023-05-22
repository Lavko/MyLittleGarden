using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ScheduledRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSchedule",
                table: "ActionRules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ScheduledTimeId",
                table: "ActionRules",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ActionRules_ScheduledTimeId",
                table: "ActionRules",
                column: "ScheduledTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionRules_ScheduledTime_ScheduledTimeId",
                table: "ActionRules",
                column: "ScheduledTimeId",
                principalTable: "ScheduledTime",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionRules_ScheduledTime_ScheduledTimeId",
                table: "ActionRules");

            migrationBuilder.DropTable(
                name: "ScheduledTime");

            migrationBuilder.DropIndex(
                name: "IX_ActionRules_ScheduledTimeId",
                table: "ActionRules");

            migrationBuilder.DropColumn(
                name: "IsSchedule",
                table: "ActionRules");

            migrationBuilder.DropColumn(
                name: "ScheduledTimeId",
                table: "ActionRules");
        }
    }
}
