using backend_stepkind.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace backend_stepkind.Data;

public class StepKindDbContext : DbContext
{
    public StepKindDbContext(DbContextOptions<StepKindDbContext> options) : base(options)
    {
    }

    public DbSet<Tutorial> Tutorials => Set<Tutorial>();
    public DbSet<TutorialChunk> TutorialChunks => Set<TutorialChunk>();
    public DbSet<SemanticSearchRow> SemanticSearchRows => Set<SemanticSearchRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Tutorial>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasMaxLength(200);
            entity.Property(x => x.Name).HasMaxLength(300).IsRequired();
            entity.Property(x => x.Url).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(5000);
            entity.Property(x => x.SearchContent).IsRequired();

            entity.HasMany(x => x.Chunks)
                .WithOne(x => x.Tutorial)
                .HasForeignKey(x => x.TutorialId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.Name);
            entity.HasIndex(x => x.CreatedAt);
        });

        modelBuilder.Entity<SemanticSearchRow>().HasNoKey();

        modelBuilder.Entity<TutorialChunk>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).IsRequired();
            entity.Property(x => x.Embedding)
                .HasColumnType("vector(1536)")
                .IsRequired();

            entity.HasIndex(x => new { x.TutorialId, x.ChunkIndex }).IsUnique();

            // Example manual index (migration SQL):
            // CREATE INDEX CONCURRENTLY IF NOT EXISTS idx_tutorial_chunks_embedding_hnsw
            // ON "TutorialChunks" USING hnsw ("Embedding" vector_cosine_ops);
        });
    }
}
