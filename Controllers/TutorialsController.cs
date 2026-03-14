using backend_stepkind.DTOs;
using backend_stepkind.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_stepkind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutorialsController : ControllerBase
{
    private readonly ITutorialService _tutorialService;
    private readonly ITutorialEmbeddingService _tutorialEmbeddingService;

    public TutorialsController(ITutorialService tutorialService, ITutorialEmbeddingService tutorialEmbeddingService)
    {
        _tutorialService = tutorialService;
        _tutorialEmbeddingService = tutorialEmbeddingService;
    }

    [HttpPost]
    public async Task<ActionResult<TutorialResponse>> CreateTutorial([FromBody] CreateTutorialRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _tutorialService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<TutorialResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var tutorials = await _tutorialService.GetAllAsync(cancellationToken);
        return Ok(tutorials);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TutorialResponse>> GetById(string id, CancellationToken cancellationToken)
    {
        var tutorial = await _tutorialService.GetByIdAsync(id, cancellationToken);
        return tutorial is null ? NotFound() : Ok(tutorial);
    }

    [HttpPost("{id}/reindex")]
    public async Task<ActionResult<ReindexTutorialResponse>> Reindex(string id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tutorialEmbeddingService.ReindexTutorialAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
