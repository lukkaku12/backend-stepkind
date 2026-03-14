using backend_stepkind.DTOs;
using backend_stepkind.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_stepkind.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ISemanticSearchService _semanticSearchService;

    public SearchController(ISemanticSearchService semanticSearchService)
    {
        _semanticSearchService = semanticSearchService;
    }

    [HttpPost("semantic")]
    public async Task<ActionResult<List<SemanticSearchResultDto>>> Search([FromBody] SemanticSearchRequest request, CancellationToken cancellationToken)
    {
        var results = await _semanticSearchService.SearchAsync(request, cancellationToken);
        return Ok(results);
    }
}
