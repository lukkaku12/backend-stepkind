namespace backend_stepkind.DTOs;

public class ReindexTutorialResponse
{
    public required string TutorialId { get; set; }
    public int ChunksCreated { get; set; }
    public DateTime ProcessedAt { get; set; }
}
