using Microsoft.Extensions.Options;
using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.WebApi.Options;
using MongoDB.Driver;

namespace ModularMonolith.Catalog.Infraestructure.Data;

internal sealed class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(IOptions<CatalogOptions> options)
    {
        var mongoClient = new MongoClient(options.Value.MongoConnectionString);
        var database = mongoClient.GetDatabase(options.Value.MongoDatabaseName);
        _collection = database.GetCollection<Product>("products");
    }

    public IEnumerable<Product> GetAll()
    {
        return _collection.Find(Builders<Product>.Filter.Empty).ToList();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetByIds(string[] idItems)
    {
        var filter = Builders<Product>.Filter.In(x => x.Id, idItems);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task Add(Product product)
    {
        await _collection.InsertOneAsync(product);
    }
}