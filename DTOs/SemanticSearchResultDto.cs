namespace backend_stepkind.DTOs;

public class SemanticSearchResultDto
{
    public required string TutorialId { get; set; }
    public required string TutorialName { get; set; }
    public required string TutorialUrl { get; set; }
    public required string MatchedChunk { get; set; }
    public double Similarity { get; set; }
}
