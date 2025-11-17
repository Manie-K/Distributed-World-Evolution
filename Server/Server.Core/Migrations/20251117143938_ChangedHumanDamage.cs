using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangedHumanDamage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -17,
                column: "Damage",
                value: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -17,
                column: "Damage",
                value: 0);
        }
    }
}
