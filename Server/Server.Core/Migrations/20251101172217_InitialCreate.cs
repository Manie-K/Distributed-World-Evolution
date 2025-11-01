using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Official = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Damage = table.Column<int>(type: "integer", nullable: false),
                    Agression = table.Column<int>(type: "integer", nullable: false),
                    ReproductionNeed = table.Column<int>(type: "integer", nullable: false),
                    MaxHunger = table.Column<int>(type: "integer", nullable: false),
                    MaxHealth = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    GraphicalRepresentationID = table.Column<int>(type: "integer", nullable: false),
                    BehaviourIDs = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "ID", "Agression", "BehaviourIDs", "Damage", "GraphicalRepresentationID", "MaxHealth", "MaxHunger", "Name", "Official", "ReproductionNeed", "Type" },
                values: new object[,]
                {
                    { -2, 70, new[] { 101, 201, 307, 401 }, 30, 2, 100, 100, "Default module", true, 40, 2 },
                    { -1, 5, new int[0], 5, 1, 100, 100, "Human module", true, 5, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
