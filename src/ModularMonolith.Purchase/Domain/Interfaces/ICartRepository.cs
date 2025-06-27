namespace ModularMonolith.Purchase.Domain.Interfaces;

internal interface ICartRepository
{
    Task<Cart?> GetByUserId(Guid userId);
    void Update(Cart cart);
}