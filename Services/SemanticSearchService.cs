using backend_stepkind.Data;
using backend_stepkind.DTOs;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace backend_stepkind.Services;

public class SemanticSearchService : ISemanticSearchService
{
    private readonly StepKindDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;

    public SemanticSearchService(StepKindDbContext dbContext, IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
    }

    public async Task<List<SemanticSearchResultDto>> SearchAsync(SemanticSearchRequest request, CancellationToken cancellationToken = default)
    {
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(request.Query, cancellationToken);
        var vector = new Vector(queryEmbedding);

        var rows = await _dbContext.SemanticSearchRows
            .FromSqlInterpolated($@"
                SELECT 
                    tc.""TutorialId"" AS ""TutorialId"",
                    t.""Name"" AS ""TutorialName"",
                    t.""Url"" AS ""TutorialUrl"",
                    tc.""Content"" AS ""MatchedChunk"",
                    (1 - (tc.""Embedding"" <=> {vector})) AS ""Similarity""
                FROM ""TutorialChunks"" tc
                INNER JOIN ""Tutorials"" t ON t.""Id"" = tc.""TutorialId""
                ORDER BY tc.""Embedding"" <=> {vector}
                LIMIT {request.TopK}")
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows.Select(x => new SemanticSearchResultDto
        {
            TutorialId = x.TutorialId,
            TutorialName = x.TutorialName,
            TutorialUrl = x.TutorialUrl,
            MatchedChunk = x.MatchedChunk,
            Similarity = x.Similarity
        }).ToList();
    }
}
