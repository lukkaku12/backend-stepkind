using backend_stepkind.Config;
using backend_stepkind.Repositories;
using backend_stepkind.Services;
using Microsoft.Extensions.Options;

namespace backend_stepkind.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStepKindServices(this IServiceCollection services)
    {
        services.AddScoped<ITutorialRepository, TutorialRepository>();
        services.AddScoped<ITutorialService, TutorialService>();
        services.AddScoped<ITutorialEmbeddingService, TutorialEmbeddingService>();
        services.AddScoped<ISemanticSearchService, SemanticSearchService>();
        services.AddSingleton<ITextChunker, TextChunker>();

        services.AddOptions<OpenAiOptions>()
            .BindConfiguration(OpenAiOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.EmbeddingModel), "Embedding model is required.")
            .Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), "OpenAI:BaseUrl must be a valid absolute URI.");

        services.AddHttpClient<IEmbeddingService, OpenAiEmbeddingService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        return services;
    }
}
