using backend_stepkind.Data;
using backend_stepkind.DTOs;
using backend_stepkind.Models;
using backend_stepkind.Repositories;
using Microsoft.EntityFrameworkCore;
using Pgvector;

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

        tutorial.SearchContent = BuildSearchContent(tutorial.Name, tutorial.Description, tutorial.Transcript);
        tutorial.UpdatedAt = DateTime.UtcNow;

        var chunks = _textChunker.Chunk(tutorial.SearchContent);

        var oldChunks = _dbContext.TutorialChunks.Where(x => x.TutorialId == tutorialId);
        _dbContext.TutorialChunks.RemoveRange(oldChunks);

        var entities = new List<TutorialChunk>();
        for (var i = 0; i < chunks.Count; i++)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(chunks[i], cancellationToken);
            entities.Add(new TutorialChunk
            {
                Id = Guid.NewGuid(),
                TutorialId = tutorialId,
                ChunkIndex = i,
                Content = chunks[i],
                Embedding = new Vector(embedding),
                CreatedAt = DateTime.UtcNow
            });
        }

        await _dbContext.TutorialChunks.AddRangeAsync(entities, cancellationToken);
        await _repository.UpdateAsync(tutorial, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new ReindexTutorialResponse
        {
            TutorialId = tutorialId,
            ChunksCreated = entities.Count,
            ProcessedAt = DateTime.UtcNow
        };
    }
}
