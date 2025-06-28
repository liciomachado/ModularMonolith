using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public interface IGetProductByIdUseCase
{
    Task<Result<ProductDetailResponse, Error>> Execute(string id);
}

public class ProductDetailResponse
{
    public string Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}


internal sealed class GetProductByIdUseCase(IProductRepository repository) : IGetProductByIdUseCase
{
    public async Task<Result<ProductDetailResponse, Error>> Execute(string id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
            return new NotFoundError("Produto não encontrado");

        return new ProductDetailResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}