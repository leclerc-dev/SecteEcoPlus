using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class Profiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicProfile_AspNetUsers_SecteUserId",
                table: "PublicProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfile_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicProfile",
                table: "PublicProfile");

            migrationBuilder.RenameTable(
                name: "PublicProfile",
                newName: "PublicProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_PublicProfile_SecteUserId",
                table: "PublicProfiles",
                newName: "IX_PublicProfiles_SecteUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicProfiles",
                table: "PublicProfiles",
                column: "PublicProfileId");

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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicProfiles_AspNetUsers_SecteUserId",
                table: "PublicProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublicProfiles",
                table: "PublicProfiles");

            migrationBuilder.RenameTable(
                name: "PublicProfiles",
                newName: "PublicProfile");

            migrationBuilder.RenameIndex(
                name: "IX_PublicProfiles_SecteUserId",
                table: "PublicProfile",
                newName: "IX_PublicProfile_SecteUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublicProfile",
                table: "PublicProfile",
                column: "PublicProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicProfile_AspNetUsers_SecteUserId",
                table: "PublicProfile",
                column: "SecteUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfile_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfile",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
