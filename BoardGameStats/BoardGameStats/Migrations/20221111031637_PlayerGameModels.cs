using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameStats.Migrations
{
    /// <inheritdoc />
    public partial class PlayerGameModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardGame",
                table: "BoardGame");

            migrationBuilder.RenameTable(
                name: "BoardGame",
                newName: "BoardGames");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardGames",
                table: "BoardGames",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardGameId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Won = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardGames",
                table: "BoardGames");

            migrationBuilder.RenameTable(
                name: "BoardGames",
                newName: "BoardGame");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardGame",
                table: "BoardGame",
                column: "Id");
        }
    }
}
