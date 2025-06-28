namespace ModularMonolith.Catalog.Domain;

internal class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public List<float> Embedding { get; set; }

    protected Product() { }

    public Product(string name, string description, decimal price, int stock)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
    }

    public Product(string id, string name, string description, decimal price, int stock, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CreatedAt = createdAt;
        UpdatedAt = null;
        Embedding = [];
    }

    public void UpdateDetails(string name, string description, decimal price)
    {
        Name = name;
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (quantity > Stock)
            throw new InvalidOperationException("Not enough stock to remove.");

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddEmbedding(List<float> embeddings)
    {
        Embedding = embeddings;
    }

    public string ToTextEmbedding()
    {
        return $"Produto: {Name}. " +
               $"Descrição: {Description}. " +
               $"Preço: R$ {Price:F2}. " +
               $"Estoque: {Stock} unidades. " +
               $"Cadastrado em: {CreatedAt:dd/MM/yyyy}.";
    }
}