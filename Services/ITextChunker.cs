namespace backend_stepkind.Services;

public interface ITextChunker
{
    IReadOnlyList<string> Chunk(string text, int maxChunkLength = 1200);
}
