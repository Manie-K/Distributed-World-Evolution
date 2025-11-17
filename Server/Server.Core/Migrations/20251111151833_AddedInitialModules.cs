using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedInitialModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -2,
                columns: new[] { "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "ReproductionNeed" },
                values: new object[] { 2, new[] { 101, 206, 305, 406 }, 2, 1, 70, 70, "Blue Plant", 6 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "ReproductionNeed", "Type" },
                values: new object[] { 1, new[] { 101, 212, 307, 406 }, 1, 0, 80, 80, "Red Plant", 10, 2 });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "ID", "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "Official", "ReproductionNeed", "Type" },
                values: new object[,]
                {
                    { -29, 0, new[] { 104, 213, 309, 403 }, 0, 28, 100, 20, "Tulip", true, 5, 4 },
                    { -28, 0, new[] { 104, 213, 309, 403 }, 0, 27, 80, 40, "Sunflower", true, 6, 4 },
                    { -27, 0, new[] { 104, 213, 309, 403 }, 0, 26, 100, 30, "Rose", true, 4, 4 },
                    { -26, 0, new[] { 104, 213, 309, 403 }, 0, 25, 70, 10, "Poppy", true, 8, 4 },
                    { -25, 0, new[] { 104, 213, 309, 403 }, 0, 24, 50, 10, "Pansy", true, 10, 4 },
                    { -24, 0, new[] { 104, 213, 309, 403 }, 0, 23, 70, 30, "Orchid", true, 4, 4 },
                    { -23, 0, new[] { 104, 213, 309, 403 }, 0, 22, 60, 20, "LilyOfTheValley", true, 8, 4 },
                    { -22, 0, new[] { 104, 213, 309, 403 }, 0, 21, 50, 10, "Lily", true, 7, 4 },
                    { -21, 0, new[] { 104, 213, 309, 403 }, 0, 20, 60, 20, "Lavender", true, 5, 4 },
                    { -20, 0, new[] { 104, 213, 309, 403 }, 0, 19, 40, 10, "Daisy", true, 6, 4 },
                    { -19, 0, new[] { 104, 213, 309, 403 }, 0, 18, 30, 10, "Daffodil", true, 7, 4 },
                    { -18, 0, new[] { 104, 213, 309, 403 }, 0, 17, 30, 10, "Cosmo", true, 7, 4 },
                    { -17, 0, new int[0], 0, 16, 100, 100, "Human", true, 0, 1 },
                    { -16, 6, new[] { 102, 202, 302, 404 }, 9, 15, 90, 80, "Red vampire", true, 2, 2 },
                    { -15, 8, new[] { 102, 209, 302, 402 }, 5, 14, 90, 80, "Blue vampire", true, 5, 2 },
                    { -14, 6, new[] { 102, 203, 302, 406 }, 7, 13, 90, 90, "Vampire", true, 3, 2 },
                    { -13, 6, new[] { 105, 201, 307, 402 }, 10, 12, 90, 80, "Darkgreen orc", true, 5, 2 },
                    { -12, 8, new[] { 105, 205, 301, 406 }, 5, 11, 90, 80, "Blue orc", true, 5, 2 },
                    { -11, 7, new[] { 101, 201, 301, 406 }, 7, 10, 90, 90, "Green orc", true, 5, 2 },
                    { -10, 5, new[] { 105, 204, 301, 405 }, 7, 9, 50, 80, "Red slime", true, 3, 2 },
                    { -9, 2, new[] { 101, 205, 306, 408 }, 3, 8, 60, 90, "Blue slime", true, 6, 2 },
                    { -8, 1, new[] { 103, 212, 304, 404 }, 1, 7, 50, 90, "Green slime", true, 8, 2 },
                    { -7, 2, new[] { 103, 207, 303, 407 }, 2, 5, 40, 50, "Brown rabbit", true, 9, 2 },
                    { -6, 3, new[] { 103, 207, 305, 407 }, 2, 4, 40, 50, "White rabbit", true, 10, 2 },
                    { -5, 7, new[] { 101, 210, 301, 407 }, 7, 3, 70, 90, "Boar", true, 6, 2 },
                    { -4, 4, new[] { 103, 208, 308, 404 }, 4, 2, 60, 80, "Pig", true, 6, 2 },
                    { -3, 4, new[] { 105, 201, 307, 406 }, 5, 6, 70, 70, "Purple Plant", true, 4, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -3);

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -2,
                columns: new[] { "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "ReproductionNeed" },
                values: new object[] { 70, new[] { 101, 201, 307, 401 }, 30, 2, 100, 100, "Default module", 40 });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "ReproductionNeed", "Type" },
                values: new object[] { 5, new int[0], 5, 16, 100, 100, "Human module", 5, 1 });
        }
    }
}
