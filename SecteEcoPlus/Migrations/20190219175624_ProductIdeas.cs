using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecteEcoPlus.Migrations
{
    public partial class ProductIdeas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "PublicProfiles",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductIdeas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorPublicProfileId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIdeas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductIdeas_PublicProfiles_AuthorPublicProfileId",
                        column: x => x.AuthorPublicProfileId,
                        principalTable: "PublicProfiles",
                        principalColumn: "PublicProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductIdeaVote",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IssuerPublicProfileId = table.Column<int>(nullable: true),
                    Direction = table.Column<int>(nullable: false),
                    ProductIdeaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIdeaVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductIdeaVote_PublicProfiles_IssuerPublicProfileId",
                        column: x => x.IssuerPublicProfileId,
                        principalTable: "PublicProfiles",
                        principalColumn: "PublicProfileId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductIdeaVote_ProductIdeas_ProductIdeaId",
                        column: x => x.ProductIdeaId,
                        principalTable: "ProductIdeas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.CreateIndex(
                name: "IX_ProductIdeas_AuthorPublicProfileId",
                table: "ProductIdeas",
                column: "AuthorPublicProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIdeaVote_IssuerPublicProfileId",
                table: "ProductIdeaVote",
                column: "IssuerPublicProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIdeaVote_ProductIdeaId",
                table: "ProductIdeaVote",
                column: "ProductIdeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductIdeaVote");

            migrationBuilder.DropTable(
                name: "ProductIdeas");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "PublicProfiles",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
