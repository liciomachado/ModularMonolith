using ModularMonolith.Core.Utils;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IRemoveItemCartUseCase
{
    Task<Result<RemoveItemCartResponse, Error>> Execute(RemoveItemCartRequest request, Guid userId);
}

public record RemoveItemCartRequest(string ItemId);
public record RemoveItemCartResponse(string CartId, string ItemId);

internal sealed class RemoveItemCartUseCase(ICartRepository cartRepository) : IRemoveItemCartUseCase
{
    public async Task<Result<RemoveItemCartResponse, Error>> Execute(RemoveItemCartRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.ItemId))
            return new BadRequestError("ItemId n�o pode ser vazio.");

        var cart = await cartRepository.GetByUserId(userId);
        if (cart is null)
            return new NotFoundError("Carrinho n�o encontrado para o usu�rio.");

        var item = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item is null)
            return new NotFoundError("Item n�o encontrado no carrinho.");

        cart.RemoveItem(request.ItemId);
        cartRepository.Update(cart);

        return new RemoveItemCartResponse(cart.Id.ToString(), request.ItemId);
    }
}