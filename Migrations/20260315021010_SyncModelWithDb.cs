using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_stepkind.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelWithDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SemanticSearchRows",
                columns: table => new
                {
                    TutorialId = table.Column<string>(type: "text", nullable: false),
                    TutorialName = table.Column<string>(type: "text", nullable: false),
                    TutorialUrl = table.Column<string>(type: "text", nullable: false),
                    MatchedChunk = table.Column<string>(type: "text", nullable: false),
                    Similarity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SemanticSearchRows");
        }
    }
}
