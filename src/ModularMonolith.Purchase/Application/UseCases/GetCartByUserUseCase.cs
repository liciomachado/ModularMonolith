using ModularMonolith.Catalog.Shared;
using ModularMonolith.Core.Utils;
using ModularMonolith.Purchase.Domain;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IGetCartByUserUseCase
{
    Task<Result<GetCartByUserResponse, Error>> Execute(Guid userId);
}

public record CartItemDto(string ItemId, int Quantity);
public record GetCartByUserResponse(string CartId, Guid UserId, DateTime CreatedAt, List<CartItemDto> Items);

internal sealed class GetCartByUserUseCase(ICartRepository cartRepository, IGetProductsSharedUseCase getProductsSharedUseCase) : IGetCartByUserUseCase
{
    public async Task<Result<GetCartByUserResponse, Error>> Execute(Guid userId)
    {
        var cart = await cartRepository.GetByUserId(userId) ?? new Cart(userId);
        var getProducts = await getProductsSharedUseCase.Execute(cart.Items.Select(i => i.ItemId).ToArray());
        var items = cart.Items.Select(i => new CartItemDto(i.ItemId, i.Quantity)).ToList();

        return new GetCartByUserResponse(
            cart.Id.ToString(),
            cart.UserId,
            cart.CreatedAt,
            items
        );
    }
}