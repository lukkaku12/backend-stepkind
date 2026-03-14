namespace backend_stepkind.Data;

public class SemanticSearchRow
{
    public string TutorialId { get; set; } = string.Empty;
    public string TutorialName { get; set; } = string.Empty;
    public string TutorialUrl { get; set; } = string.Empty;
    public string MatchedChunk { get; set; } = string.Empty;
    public double Similarity { get; set; }
}
