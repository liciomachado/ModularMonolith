namespace ModularMonolith.Catalog.Domain.Interfaces;

internal interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetByIds(Guid[] idItems);
    Task Add(Product product);
}