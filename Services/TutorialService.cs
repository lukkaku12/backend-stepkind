using backend_stepkind.DTOs;
using backend_stepkind.Models;
using backend_stepkind.Repositories;

namespace backend_stepkind.Services;

public class TutorialService : ITutorialService
{
    private readonly ITutorialRepository _repository;
    private readonly ITutorialEmbeddingService _embeddingService;

    public TutorialService(ITutorialRepository repository, ITutorialEmbeddingService embeddingService)
    {
        _repository = repository;
        _embeddingService = embeddingService;
    }

    public async Task<TutorialResponse> CreateAsync(CreateTutorialRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (exists is not null)
        {
            throw new InvalidOperationException($"Tutorial with id '{request.Id}' already exists.");
        }

        var now = DateTime.UtcNow;
        var tutorial = new Tutorial
        {
            Id = request.Id,
            Name = request.Name,
            Url = request.Url,
            Description = request.Description,
            Transcript = request.Transcript,
            SearchContent = _embeddingService.BuildSearchContent(request.Name, request.Description, request.Transcript),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repository.CreateAsync(tutorial, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapResponse(tutorial);
    }

    public async Task<List<TutorialResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tutorials = await _repository.GetAllAsync(cancellationToken);
        return tutorials.Select(MapResponse).ToList();
    }

    public async Task<TutorialResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var tutorial = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        return tutorial is null ? null : MapResponse(tutorial);
    }

    private static TutorialResponse MapResponse(Tutorial tutorial)
        => new()
        {
            Id = tutorial.Id,
            Name = tutorial.Name,
            Url = tutorial.Url,
            Description = tutorial.Description,
            Transcript = tutorial.Transcript,
            SearchContent = tutorial.SearchContent,
            CreatedAt = tutorial.CreatedAt,
            UpdatedAt = tutorial.UpdatedAt
        };
}
