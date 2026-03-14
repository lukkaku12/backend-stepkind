using System.Text;

namespace backend_stepkind.Services;

public class TextChunker : ITextChunker
{
    public IReadOnlyList<string> Chunk(string text, int maxChunkLength = 1200)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var cleanText = text.Replace("\r", "").Trim();
        var paragraphs = cleanText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var chunks = new List<string>();
        var current = new StringBuilder();

        foreach (var paragraph in paragraphs)
        {
            if (paragraph.Length > maxChunkLength)
            {
                FlushCurrent(chunks, current);
                chunks.AddRange(SplitLongParagraph(paragraph, maxChunkLength));
                continue;
            }

            if (current.Length + paragraph.Length + 2 > maxChunkLength)
            {
                FlushCurrent(chunks, current);
            }

            if (current.Length > 0)
            {
                current.Append("\n\n");
            }

            current.Append(paragraph);
        }

        FlushCurrent(chunks, current);
        return chunks.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }

    private static List<string> SplitLongParagraph(string paragraph, int maxChunkLength)
    {
        var words = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var chunks = new List<string>();
        var current = new StringBuilder();

        foreach (var word in words)
        {
            if (current.Length + word.Length + 1 > maxChunkLength)
            {
                if (current.Length > 0)
                {
                    chunks.Add(current.ToString().Trim());
                    current.Clear();
                }
            }

            if (current.Length > 0)
            {
                current.Append(' ');
            }

            current.Append(word);
        }

        if (current.Length > 0)
        {
            chunks.Add(current.ToString().Trim());
        }

        return chunks;
    }

    private static void FlushCurrent(List<string> chunks, StringBuilder current)
    {
        if (current.Length == 0)
        {
            return;
        }

        chunks.Add(current.ToString().Trim());
        current.Clear();
    }
}
