using backend_stepkind.Config;
using backend_stepkind.Repositories;
using backend_stepkind.Services;

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

        services.AddHttpClient<IEmbeddingService, OpenAiEmbeddingService>(client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/v1/");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        services.AddOptions<OpenAiOptions>()
            .BindConfiguration(OpenAiOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.EmbeddingModel), "OpenAI embedding model is required.");

        return services;
    }
}
