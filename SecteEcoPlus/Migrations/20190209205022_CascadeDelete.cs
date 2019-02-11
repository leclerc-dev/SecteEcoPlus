using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicProfiles_AspNetUsers_SecteUserId",
                table: "PublicProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicProfiles_AspNetUsers_SecteUserId",
                table: "PublicProfiles",
                column: "SecteUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicProfiles_AspNetUsers_SecteUserId",
                table: "PublicProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicProfiles_AspNetUsers_SecteUserId",
                table: "PublicProfiles",
                column: "SecteUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
