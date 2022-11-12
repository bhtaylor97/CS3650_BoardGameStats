using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoardGameStats.Migrations
{
    /// <inheritdoc />
    public partial class NumPlaysPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumPlays",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumPlays",
                table: "Players");
        }
    }
}
