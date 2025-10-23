using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedDataWithNewModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "ID", "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "Name", "Official", "ReproductionNeed", "Type" },
                values: new object[] { -2, 7, new[] { 3, 2, 69 }, 3, 2, "Default module", true, 4, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -2);
        }
    }
}
