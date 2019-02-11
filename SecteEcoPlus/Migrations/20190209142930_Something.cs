using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class Something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
