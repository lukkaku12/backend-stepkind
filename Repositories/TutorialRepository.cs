using backend_stepkind.Data;
using backend_stepkind.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_stepkind.Repositories;

public class TutorialRepository : ITutorialRepository
{
    private readonly StepKindDbContext _dbContext;

    public TutorialRepository(StepKindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Tutorial tutorial, CancellationToken cancellationToken = default)
        => await _dbContext.Tutorials.AddAsync(tutorial, cancellationToken);

    public async Task<Tutorial?> GetByIdAsync(string id, bool includeChunks = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Tutorials.AsQueryable();
        if (includeChunks)
        {
            query = query.Include(x => x.Chunks);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<Tutorial>> GetAllAsync(CancellationToken cancellationToken = default)
        => _dbContext.Tutorials
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

    public Task UpdateAsync(Tutorial tutorial, CancellationToken cancellationToken = default)
    {
        _dbContext.Tutorials.Update(tutorial);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tutorial tutorial, CancellationToken cancellationToken = default)
    {
        _dbContext.Tutorials.Remove(tutorial);
        return Task.CompletedTask;
    }

    public Task<List<TutorialChunk>> GetChunksByTutorialIdAsync(string tutorialId, CancellationToken cancellationToken = default)
        => _dbContext.TutorialChunks
            .Where(x => x.TutorialId == tutorialId)
            .OrderBy(x => x.ChunkIndex)
            .ToListAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
