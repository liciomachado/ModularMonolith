using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using ModularMonolith.Core.WebApi.Options;

namespace ModularMonolith.Catalog.Application.Services;

public interface IEmbeddingService
{
    Task<IList<float>> GenerateEmbeddingAsync(string text);
}

internal sealed class EmbeddingService(IOptions<CatalogOptions> catalogOptions) : IEmbeddingService
{
    private readonly string _openAiKey = catalogOptions.Value.OpenaiApiKey;
    private readonly string _model = catalogOptions.Value.ModelEmbedding;

    public async Task<IList<float>> GenerateEmbeddingAsync(string text)
    {
#pragma warning disable SKEXP0010
        var textEmbeddingGenerationService = new OpenAITextEmbeddingGenerationService(
            modelId: _model,          // Name of the embedding model, e.g. "text-embedding-ada-002".
            apiKey: _openAiKey
        );

        var embedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(text);
        return embedding.ToArray();
    }
}