namespace ModularMonolith.Catalog.Domain;

internal sealed class HistoryProductUser
{
    public Guid UserId { get; set; }

    public List<ProductHistory> Products { get; set; }

    public HistoryProductUser(Guid userId, string productId)
    {
        UserId = userId;
        Products = new List<ProductHistory> { new(productId) };
    }

    public void AddProduct(string productId)
    {
        if (Products.All(p => p.PrductId != productId))
        {
            Products.Add(new ProductHistory(productId));
        }
        else
        {
            var existingProduct = Products.First(p => p.PrductId == productId);
            existingProduct.CreatedAt = DateTime.Now; // Atualiza o timestamp se já existir
        }
    }

    public class ProductHistory(string prductId)
    {
        public string PrductId { get; set; } = prductId;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}