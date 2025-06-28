using ModularMonolith.Core.Utils;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IUpdateItemCartUseCase
{
    Task<Result<UpdateItemCartResponse, Error>> Execute(UpdateItemCartRequest request, Guid userId);
}

public record UpdateItemCartRequest(string ItemId, int Quantity);
public record UpdateItemCartResponse(Guid CartId, string ItemId, int Quantity);

internal sealed class UpdateItemCartUseCase(ICartRepository cartRepository) : IUpdateItemCartUseCase
{
    public async Task<Result<UpdateItemCartResponse, Error>> Execute(UpdateItemCartRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.ItemId))
            return new BadRequestError("ItemId não pode ser vazio.");
        if (request.Quantity < 0)
            return new BadRequestError("Quantidade não pode ser negativa.");

        var cart = await cartRepository.GetByUserId(userId);
        if (cart is null)
            return new NotFoundError("Carrinho não encontrado para o usuário.");

        var item = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
        if (item is null)
            return new NotFoundError("Item não encontrado no carrinho.");

        cart.UpdateItemQuantity(request.ItemId, request.Quantity);
        cartRepository.Update(cart);

        return new UpdateItemCartResponse(cart.Id, request.ItemId, request.Quantity);
    }
}