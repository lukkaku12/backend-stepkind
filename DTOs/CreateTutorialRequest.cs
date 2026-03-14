using System.ComponentModel.DataAnnotations;

namespace backend_stepkind.DTOs;

public class CreateTutorialRequest
{
    [Required]
    [MaxLength(200)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(300)]
    public required string Name { get; set; }

    [Required]
    [Url]
    [MaxLength(1000)]
    public required string Url { get; set; }

    [MaxLength(5000)]
    public string? Description { get; set; }

    public string? Transcript { get; set; }
}
