namespace ModularMonolith.Catalog.Domain.Interfaces;

internal interface IVectorDatabaseRepository
{
    Task CreateCollectionIfNotExistsAsync(int vectorSize);
    Task UpsertProductAsync(Product product);
    Task<List<Product>> SearchSimilarProductsAsync(string productId, List<float> embedding, List<HistoryProductUser.ProductHistory> excludeProducts, int topK = 5);
}