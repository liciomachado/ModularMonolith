using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ModularMonolith.Catalog.Infraestructure.Data;

internal class VectorDatabaseRepository(HttpClient httpClient) : IVectorDatabaseRepository
{
    private readonly string _collectionName = "products";

    public async Task UpsertProductAsync(Product product)
    {
        if (product.Embedding == null || !product.Embedding.Any())
            throw new ArgumentException("Produto sem embedding gerado.");

        var payload = new
        {
            points = new[]
            {
                new
                {
                    id = product.Id,
                    vector = product.Embedding,
                    payload = new
                    {
                        productId = product.Id,
                        name = product.Name,
                        description = product.Description,
                        price = product.Price,
                        stock = product.Stock,
                        createdAt = product.CreatedAt
                    }
                }
            }
        };

        var response = await httpClient.PutAsJsonAsync($"/collections/products/points", payload);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao indexar produto no Qdrant: {response.StatusCode} - {content}");
        }
    }


    public async Task CreateCollectionIfNotExistsAsync(int vectorSize)
    {
        var response = await httpClient.PutAsJsonAsync($"/collections/{_collectionName}", new
        {
            vectors = new
            {
                size = vectorSize,
                distance = "Cosine" // ou Euclidean, Dot
            }
        });

        if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar coleção Qdrant: {response.StatusCode} - {content}");
        }
    }

    public async Task<List<Product>> SearchSimilarProductsAsync(string productId, List<float> embedding,
        List<HistoryProductUser.ProductHistory> excludeProducts, int topK = 5)
    {
        // 2. Criar filtro de exclusão
        var excludedIds = excludeProducts?.Select(p => p.PrductId)?.ToList() ?? [];

        // 2. Realizar a busca por similaridade
        var searchRequest = new
        {
            vector = embedding,
            top = topK + excludedIds.Count, // aumenta para compensar exclusão
            with_payload = true,
            filter = new
            {
                must_not = new[]
                {
                    new
                    {
                        key = "productId",
                        match = new
                        {
                            any = excludedIds
                        }
                    }
                }
            }
        };
        var json = JsonSerializer.Serialize(searchRequest);
        var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
        var searchResponse = await httpClient.PostAsync("/collections/products/points/search", contentJson);

        if (!searchResponse.IsSuccessStatusCode)
        {
            var content = await searchResponse.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar similares no Qdrant: {searchResponse.StatusCode} - {content}");
        }

        var searchResult = await searchResponse.Content.ReadFromJsonAsync<QdrantSearchResult>();

        return searchResult?.Result?
            .Where(p => p.Id.ToString() != productId) // remove o próprio
            .Take(topK)
            .Select(p => new Product(p.Id, p.Payload.Name,
                p.Payload.Description, p.Payload.Price, p.Payload.Stock, p.Payload.CreatedAt)
            )
            .ToList() ?? new List<Product>();
    }

}

public class QdrantPointsResponse
{
    public List<QdrantPointResult> Result { get; set; }
}

public class QdrantPointResult
{
    public object Id { get; set; }
    public List<float> Vector { get; set; }
}

public class QdrantSearchResult
{
    public List<QdrantScoredPoint> Result { get; set; }
}

public class QdrantScoredPoint
{
    public string Id { get; set; }
    public float Score { get; set; }
    public ProductPayload Payload { get; set; }
}

public class ProductPayload
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
}
