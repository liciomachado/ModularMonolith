namespace ModularMonolith.Catalog.Domain.Interfaces;

internal interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Task<Product?> GetByIdAsync(string id);
    Task<List<Product>> GetByIds(string[] idItems);
    Task Add(Product product);
}