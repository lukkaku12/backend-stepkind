using backend_stepkind.Data;
using backend_stepkind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_stepkind.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutorialController : ControllerBase
{
    private readonly AppDbContext _db;

    public TutorialController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTutorial(string id)
    {
        var result = await _db.Tutorials.FirstOrDefaultAsync(t => t.Id == id);

         if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTutorials()
    {
        var results = _db.Tutorials.ToListAsync();

        if (results is null)
            return NotFound();
        
        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTutorial([FromBody] Tutorial tutorial)
    {
        _db.Tutorials.Add(tutorial);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTutorial),
            new { id = tutorial.Id },
            tutorial
        );
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTutorial(string id)
    {
      var tutorial = await _db.Tutorials.FirstOrDefaultAsync(t => t.Id == id);  

    if (tutorial is null)
        return NotFound();

    _db.Tutorials.Remove(tutorial);
    await _db.SaveChangesAsync();
    

      return NoContent();
    }
}
