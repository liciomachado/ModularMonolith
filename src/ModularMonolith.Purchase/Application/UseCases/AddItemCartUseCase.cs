using ModularMonolith.Core.Utils;

namespace ModularMonolith.Purchase.Application.UseCases;

public interface IAddItemCartUseCase
{
    Task<Result<AddItemCartResponse, Error>> Execute(AddItemCartRequest request);
}
public record AddItemCartRequest(string ItemId, int Quantity);
public record AddItemCartResponse(string CartId, string ItemId, int Quantity);
internal sealed class AddItemCartUseCase : IAddItemCartUseCase
{
    public Task<Result<AddItemCartResponse, Error>> Execute(AddItemCartRequest request)
    {
        throw new NotImplementedException();
    }
}