using backend_stepkind.DTOs;

namespace backend_stepkind.Services;

public interface ISemanticSearchService
{
    Task<List<SemanticSearchResultDto>> SearchAsync(SemanticSearchRequest request, CancellationToken cancellationToken = default);
}
