using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public interface IGetSimilarProductsUseCase
{
    Task<Result<ProductResponse[], Error>> Execute(Guid userId, string productId, int topK = 5);
}

internal sealed class GetSimilarProductsUseCase(
    IProductRepository productRepository,
    IVectorDatabaseRepository vectorDatabaseRepository,
    IHistoryProductUserRepository historyProductUserRepository
) : IGetSimilarProductsUseCase
{
    public async Task<Result<ProductResponse[], Error>> Execute(Guid userId, string productId, int topK = 5)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
            return new NotFoundError("Produto de referência não encontrado.");

        if (product.Embedding.Count == 0)
            return new BadRequestError("Produto de referência não possui embedding.");

        var productsExcluded = await historyProductUserRepository.GetByUserIdAsync(userId);
        if (productsExcluded is null)
        {
            var userHistory = new HistoryProductUser(userId, productId);
            await historyProductUserRepository.AddAsync(userHistory);
            productsExcluded = userHistory;
        }
        else
        {
            productsExcluded.AddProduct(productId);
        }

        var similarProducts = await vectorDatabaseRepository.SearchSimilarProductsAsync(productId, product.Embedding, productsExcluded.Products, topK);
        var dtos = similarProducts.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        }).ToArray();
        return dtos;
    }
}