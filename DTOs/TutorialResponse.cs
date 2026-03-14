namespace backend_stepkind.DTOs;

public class TutorialResponse
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public string? Description { get; set; }
    public string? Transcript { get; set; }
    public required string SearchContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
