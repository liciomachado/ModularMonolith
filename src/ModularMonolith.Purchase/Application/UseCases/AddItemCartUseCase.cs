using ModularMonolith.Core.Utils;
using ModularMonolith.Purchase.Domain;
using ModularMonolith.Purchase.Domain.Interfaces;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IAddItemCartUseCase
{
    Task<Result<AddItemCartResponse, Error>> Execute(AddItemCartRequest request, Guid userId);
}

public record AddItemCartRequest(Guid ItemId, int Quantity);
public record AddItemCartResponse(Guid CartId, Guid ItemId, int Quantity);

internal sealed class AddItemCartUseCase(ICartRepository cartRepository) : IAddItemCartUseCase
{
    public async Task<Result<AddItemCartResponse, Error>> Execute(AddItemCartRequest request, Guid userId)
    {
        if (request.ItemId == Guid.Empty)
            return new BadRequestError("ItemId não pode ser vazio.");
        if (request.Quantity <= 0)
            return new BadRequestError("Quantidade deve ser maior que zero.");

        var cart = await cartRepository.GetByUserId(userId) ?? new Cart(userId);
        cart.AddItem(request.ItemId, request.Quantity);
        cartRepository.Update(cart);

        var response = new AddItemCartResponse(cart.Id, request.ItemId, request.Quantity);
        return response;
    }
}