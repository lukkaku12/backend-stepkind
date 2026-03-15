namespace backend_stepkind.Config;

public class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1/";
    public string EmbeddingModel { get; set; } = "openai/text-embedding-3-small";
    public string? HttpReferer { get; set; }
    public string? AppName { get; set; }
}
