using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public interface ISyncAllProductsToVectorDbUseCase
{
    Task<Result<SyncAllProductsToVectorDbResponse, Error>> Execute();
}

public record SyncAllProductsToVectorDbResponse(int TotalProducts, int SuccessCount, int ErrorCount, List<string> Errors);

internal sealed class SyncAllProductsToVectorDbUseCase(
    IProductRepository productRepository,
    IVectorDatabaseRepository vectorDatabaseRepository
) : ISyncAllProductsToVectorDbUseCase
{
    public async Task<Result<SyncAllProductsToVectorDbResponse, Error>> Execute()
    {
        var products = productRepository.GetAll().ToList();
        int success = 0, error = 0;
        var errors = new List<string>();

        foreach (var product in products)
        {
            try
            {
                await vectorDatabaseRepository.UpsertProductAsync(product);
                success++;
            }
            catch (Exception ex)
            {
                error++;
                errors.Add($"Id: {product.Id} - {ex.Message}");
            }
        }

        return new SyncAllProductsToVectorDbResponse(products.Count, success, error, errors);
    }
}