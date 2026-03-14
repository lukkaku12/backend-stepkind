using backend_stepkind.DTOs;

namespace backend_stepkind.Services;

public interface ITutorialService
{
    Task<TutorialResponse> CreateAsync(CreateTutorialRequest request, CancellationToken cancellationToken = default);
    Task<List<TutorialResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TutorialResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
