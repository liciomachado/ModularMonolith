using ModularMonolith.Catalog.Shared;
using ModularMonolith.Core.Utils;
using ModularMonolith.Purchase.Domain;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IGetCartByUserUseCase
{
    Task<Result<GetCartByUserResponse, Error>> Execute(Guid userId);
}

public record CartItemDto(string ItemId, string Name, string Description, int Quantity, decimal Price);
public record GetCartByUserResponse(string CartId, List<CartItemDto> Items, decimal Total);

internal sealed class GetCartByUserUseCase(ICartRepository cartRepository, IGetProductsSharedUseCase getProductsSharedUseCase) : IGetCartByUserUseCase
{
    public async Task<Result<GetCartByUserResponse, Error>> Execute(Guid userId)
    {
        var cart = await cartRepository.GetByUserId(userId) ?? new Cart(userId);
        if (!cart.Items.Any())
            return new GetCartByUserResponse(cart.Id.ToString(), [], 0);

        var getProducts = await getProductsSharedUseCase.Execute(cart.Items.Select(i => i.ItemId).ToArray());
        if (getProducts.IsFailure)
            return new BadRequestError("Nao foi possivel obter os itens do carrinho");

        var items = getProducts.Value!.Select(i =>
        {
            var quantity = cart.Items.FirstOrDefault(ci => ci.ItemId == i.Id)?.Quantity ?? 0;
            return new CartItemDto(i.Id, i.Name, i.Description, quantity, i.Price);
        }).ToList();

        var total = items.Sum(i => i.Price * i.Quantity);
        return new GetCartByUserResponse(
            cart.Id.ToString(),
            items,
            total
        );
    }
}