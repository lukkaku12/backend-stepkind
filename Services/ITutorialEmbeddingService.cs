using backend_stepkind.DTOs;

namespace backend_stepkind.Services;

public interface ITutorialEmbeddingService
{
    string BuildSearchContent(string name, string? description, string? transcript);
    Task<ReindexTutorialResponse> ReindexTutorialAsync(string tutorialId, CancellationToken cancellationToken = default);
}
