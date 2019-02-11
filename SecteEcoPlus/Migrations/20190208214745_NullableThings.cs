using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class NullableThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "WebsiteReviews",
                nullable: true,
                oldClrType: typeof(int));

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
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "WebsiteReviews",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WebsiteReviews_PublicProfiles_AuthorId",
                table: "WebsiteReviews",
                column: "AuthorId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
