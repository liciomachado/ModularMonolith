using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public class ProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public interface IGetProdutsUseCase
{
    Task<Result<ProductResponse[], Error>> Execute();
}

internal sealed class GetProdutsUseCase(IProductRepository repository) : IGetProdutsUseCase
{
    public async Task<Result<ProductResponse[], Error>> Execute()
    {
        var products = repository.GetAll();
        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        }).ToArray();
    }
}