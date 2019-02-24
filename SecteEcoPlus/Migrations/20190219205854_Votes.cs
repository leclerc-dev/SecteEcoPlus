using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class Votes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdeaVote_PublicProfiles_IssuerPublicProfileId",
                table: "ProductIdeaVote");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdeaVote_ProductIdeas_ProductIdeaId",
                table: "ProductIdeaVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIdeaVote",
                table: "ProductIdeaVote");

            migrationBuilder.RenameTable(
                name: "ProductIdeaVote",
                newName: "ProductIdeaVotes");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdeaVote_ProductIdeaId",
                table: "ProductIdeaVotes",
                newName: "IX_ProductIdeaVotes_ProductIdeaId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdeaVote_IssuerPublicProfileId",
                table: "ProductIdeaVotes",
                newName: "IX_ProductIdeaVotes_IssuerPublicProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIdeaVotes",
                table: "ProductIdeaVotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdeaVotes_PublicProfiles_IssuerPublicProfileId",
                table: "ProductIdeaVotes",
                column: "IssuerPublicProfileId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdeaVotes_ProductIdeas_ProductIdeaId",
                table: "ProductIdeaVotes",
                column: "ProductIdeaId",
                principalTable: "ProductIdeas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdeaVotes_PublicProfiles_IssuerPublicProfileId",
                table: "ProductIdeaVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIdeaVotes_ProductIdeas_ProductIdeaId",
                table: "ProductIdeaVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIdeaVotes",
                table: "ProductIdeaVotes");

            migrationBuilder.RenameTable(
                name: "ProductIdeaVotes",
                newName: "ProductIdeaVote");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdeaVotes_ProductIdeaId",
                table: "ProductIdeaVote",
                newName: "IX_ProductIdeaVote_ProductIdeaId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIdeaVotes_IssuerPublicProfileId",
                table: "ProductIdeaVote",
                newName: "IX_ProductIdeaVote_IssuerPublicProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIdeaVote",
                table: "ProductIdeaVote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdeaVote_PublicProfiles_IssuerPublicProfileId",
                table: "ProductIdeaVote",
                column: "IssuerPublicProfileId",
                principalTable: "PublicProfiles",
                principalColumn: "PublicProfileId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIdeaVote_ProductIdeas_ProductIdeaId",
                table: "ProductIdeaVote",
                column: "ProductIdeaId",
                principalTable: "ProductIdeas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
