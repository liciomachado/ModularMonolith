using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public interface IGetSimilarProductsUseCase
{
    Task<Result<ProductResponse[], Error>> Execute(string productId, int topK = 5);
}

internal sealed class GetSimilarProductsUseCase(
    IProductRepository productRepository,
    IVectorDatabaseRepository vectorDatabaseRepository
) : IGetSimilarProductsUseCase
{
    public async Task<Result<ProductResponse[], Error>> Execute(string productId, int topK = 5)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
            return new NotFoundError("Produto de referência não encontrado.");

        if (product.Embedding == null || product.Embedding.Count == 0)
            return new BadRequestError("Produto de referência não possui embedding.");

        var similarProducts = await vectorDatabaseRepository.SearchSimilarProductsAsync(productId, product.Embedding, topK);

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