using backend_stepkind.Data;
using backend_stepkind.DTOs;
using backend_stepkind.Models;
using backend_stepkind.Repositories;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using System.Globalization;

namespace backend_stepkind.Services;

public class TutorialEmbeddingService : ITutorialEmbeddingService
{
    private readonly ITutorialRepository _repository;
    private readonly ITextChunker _textChunker;
    private readonly IEmbeddingService _embeddingService;
    private readonly StepKindDbContext _dbContext;

    public TutorialEmbeddingService(
        ITutorialRepository repository,
        ITextChunker textChunker,
        IEmbeddingService embeddingService,
        StepKindDbContext dbContext)
    {
        _repository = repository;
        _textChunker = textChunker;
        _embeddingService = embeddingService;
        _dbContext = dbContext;
    }

    public string BuildSearchContent(string name, string? description, string? transcript)
        => string.Join("\n\n", new[] { name, description, transcript }
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim()));

    public async Task<ReindexTutorialResponse> ReindexTutorialAsync(string tutorialId, CancellationToken cancellationToken = default)
    {
        var tutorial = await _repository.GetByIdAsync(tutorialId, cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException($"Tutorial '{tutorialId}' was not found.");

        var now = DateTime.UtcNow;
        var searchContent = BuildSearchContent(tutorial.Name, tutorial.Description, tutorial.Transcript);

        var chunks = _textChunker.Chunk(searchContent);

        var createdChunkIds = new List<Guid>();
        await using var tx = await _dbContext.Database.BeginTransactionAsync(CancellationToken.None);

        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $@"DELETE FROM ""TutorialChunks"" WHERE ""TutorialId"" = {tutorialId};",
            CancellationToken.None);

        for (var i = 0; i < chunks.Count; i++)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(chunks[i], cancellationToken);
            var chunkId = Guid.NewGuid();
            createdChunkIds.Add(chunkId);

            var vectorLiteral = $"[{string.Join(",", embedding.Select(v => v.ToString("G9", CultureInfo.InvariantCulture)))}]";
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $@"INSERT INTO ""TutorialChunks"" (""Id"", ""TutorialId"", ""ChunkIndex"", ""Content"", ""Embedding"", ""CreatedAt"")
                   VALUES ({chunkId}, {tutorialId}, {i}, {chunks[i]}, CAST({vectorLiteral} AS vector), {now});",
                CancellationToken.None);
        }

        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            $@"UPDATE ""Tutorials""
               SET ""SearchContent"" = {searchContent},
                   ""UpdatedAt"" = {now}
               WHERE ""Id"" = {tutorialId};",
            CancellationToken.None);

        await tx.CommitAsync(CancellationToken.None);

        return new ReindexTutorialResponse
        {
            TutorialId = tutorialId,
            ChunksCreated = createdChunkIds.Count,
            ProcessedAt = DateTime.UtcNow
        };
    }
}
