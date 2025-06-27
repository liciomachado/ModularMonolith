using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;

namespace ModularMonolith.Catalog.Infraestructure.Data;

internal sealed class ProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new()
    {
        new Product("Notebook", "Notebook Dell Inspiron", 4500.00m, 10),
        new Product("Mouse", "Mouse Logitech Wireless", 150.00m, 50),
        new Product("Teclado", "Teclado Mecânico RGB", 350.00m, 20)
    };

    public IEnumerable<Product> GetAll() => _products;

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return _products.FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<Product>> GetByIds(Guid[] idItems)
    {
        return _products.Where(x => idItems.Contains(x.Id)).ToList();
    }
}