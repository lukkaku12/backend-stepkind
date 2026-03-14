using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace backend_stepkind.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:vector", ",,");

        migrationBuilder.CreateTable(
            name: "Tutorials",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                Transcript = table.Column<string>(type: "text", nullable: true),
                SearchContent = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tutorials", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TutorialChunks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TutorialId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                ChunkIndex = table.Column<int>(type: "integer", nullable: false),
                Content = table.Column<string>(type: "text", nullable: false),
                Embedding = table.Column<Vector>(type: "vector(1536)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TutorialChunks", x => x.Id);
                table.ForeignKey(
                    name: "FK_TutorialChunks_Tutorials_TutorialId",
                    column: x => x.TutorialId,
                    principalTable: "Tutorials",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_TutorialChunks_TutorialId_ChunkIndex",
            table: "TutorialChunks",
            columns: new[] { "TutorialId", "ChunkIndex" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tutorials_CreatedAt",
            table: "Tutorials",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Tutorials_Name",
            table: "Tutorials",
            column: "Name");

        migrationBuilder.Sql(@"
            CREATE INDEX IF NOT EXISTS idx_tutorial_chunks_embedding_hnsw
            ON \"TutorialChunks\" USING hnsw (\"Embedding\" vector_cosine_ops);");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "TutorialChunks");
        migrationBuilder.DropTable(name: "Tutorials");
    }
}
