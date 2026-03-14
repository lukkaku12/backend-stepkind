using backend_stepkind.Models;

namespace backend_stepkind.Repositories;

public interface ITutorialRepository
{
    Task CreateAsync(Tutorial tutorial, CancellationToken cancellationToken = default);
    Task<Tutorial?> GetByIdAsync(string id, bool includeChunks = false, CancellationToken cancellationToken = default);
    Task<List<Tutorial>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Tutorial tutorial, CancellationToken cancellationToken = default);
    Task DeleteAsync(Tutorial tutorial, CancellationToken cancellationToken = default);
    Task<List<TutorialChunk>> GetChunksByTutorialIdAsync(string tutorialId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
