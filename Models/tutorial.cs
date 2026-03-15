using System.ComponentModel.DataAnnotations;

namespace backend_stepkind.Models;

public class Tutorial
{
    [Required]
    [MaxLength(200)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(300)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(1000)]
    [Url]
    public required string Url { get; set; }

    [MaxLength(5000)]
    public string? Description { get; set; }

    public string? Transcript { get; set; }

    [Required]
    public string SearchContent { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<TutorialChunk> Chunks { get; set; } = new List<TutorialChunk>();
}
