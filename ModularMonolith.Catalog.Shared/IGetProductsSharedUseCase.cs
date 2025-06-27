using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Shared;

public interface IGetProductsSharedUseCase
{
    Task<Result<GetProductsSharedResponse[], Error>> Execute(Guid[] idItems);
}

public record GetProductsSharedResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}