using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Catalog.Shared;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Infraestructure.Shared;

internal sealed class GetProductsImplementation(IProductRepository productRepository) : IGetProductsSharedUseCase
{
    public async Task<Result<GetProductsSharedResponse[], Error>> Execute(string[] idItems)
    {
        var products = await productRepository.GetByIds(idItems);

        return products.Select(p => new GetProductsSharedResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        }).ToArray();
    }
}