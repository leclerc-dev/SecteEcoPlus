using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class ExperienceDebt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceDebt",
                table: "PublicProfiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceDebt",
                table: "PublicProfiles");
        }
    }
}
