using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using backend_stepkind.Config;
using Microsoft.Extensions.Options;

namespace backend_stepkind.Services;

public class OpenAiEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;
    private readonly ILogger<OpenAiEmbeddingService> _logger;

    public OpenAiEmbeddingService(HttpClient httpClient, IOptions<OpenAiOptions> options, ILogger<OpenAiEmbeddingService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key is missing. Configure OpenAI:ApiKey or OPENAI__APIKEY.");
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Input text for embedding cannot be empty.", nameof(text));
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "embeddings");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            model = _options.EmbeddingModel,
            input = text
        }), Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("OpenAI embedding request failed: {StatusCode} - {Body}", response.StatusCode, content);
            throw new InvalidOperationException($"Failed to generate embedding. Status: {response.StatusCode}");
        }

        using var doc = JsonDocument.Parse(content);
        var embedding = doc.RootElement
            .GetProperty("data")[0]
            .GetProperty("embedding")
            .EnumerateArray()
            .Select(x => x.GetSingle())
            .ToArray();

        return embedding;
    }
}
