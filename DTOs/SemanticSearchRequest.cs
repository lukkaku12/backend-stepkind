using System.ComponentModel.DataAnnotations;

namespace backend_stepkind.DTOs;

public class SemanticSearchRequest
{
    [Required]
    [MinLength(2)]
    public required string Query { get; set; }

    [Range(1, 20)]
    public int TopK { get; set; } = 5;
}
