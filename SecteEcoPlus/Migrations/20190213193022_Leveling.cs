using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class Leveling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "PublicProfiles",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "PublicProfiles");
        }
    }
}
