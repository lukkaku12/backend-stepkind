using Pgvector;
using System.ComponentModel.DataAnnotations;

namespace backend_stepkind.Models;

public class TutorialChunk
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string TutorialId { get; set; }

    public Tutorial Tutorial { get; set; } = null!;

    public int ChunkIndex { get; set; }

    [Required]
    public required string Content { get; set; }

    public Vector Embedding { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
