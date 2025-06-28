namespace ModularMonolith.Purchase.Domain;

internal class Cart
{
    public Guid Id { get; private set; }
    public Guid UserId { get; }
    public DateTime CreatedAt { get; set; }
    public List<CartItem> Items { get; }

    public Cart(Guid userId)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Items = [];
        UserId = userId;
    }
    public void AddItem(string itemId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(itemId))
            throw new ArgumentException("ItemId cannot be null or empty.", nameof(itemId));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        var existingItem = Items.FirstOrDefault(i => i.ItemId == itemId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new CartItem(itemId, quantity));
        }
    }

    public void RemoveItem(string itemId)
    {
        var item = Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public void UpdateItemQuantity(string itemId, int quantity)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

        var item = Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null)
            throw new InvalidOperationException("Item not found in cart.");

        if (quantity == 0)
        {
            Items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }
    }

    public void Clear()
    {
        Items.Clear();
    }
}

internal class CartItem
{
    public string ItemId { get; set; }
    public int Quantity { get; set; }
    public CartItem(string itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }
}