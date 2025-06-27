using ModularMonolith.Purchase.Domain;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Infraestructure.Data;

internal sealed class CartRepository : ICartRepository
{
    private static readonly List<Cart> _carts = new();

    // Para simplificação, sempre retorna o primeiro carrinho ou cria um novo
    public Task<Cart?> GetByUserId(Guid userId)
    {
        var cart = _carts.FirstOrDefault(x => x.UserId == userId);
        return Task.FromResult(cart);
    }

    public void Update(Cart cart)
    {
        if (_carts.Any(x => x.UserId == cart.UserId))
            return;

        _carts.Add(cart);
    }
}