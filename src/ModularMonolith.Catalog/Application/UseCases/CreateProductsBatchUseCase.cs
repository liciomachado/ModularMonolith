using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public record CreateProductsBatchRequest(List<CreateProductRequest> Products);
public record CreateProductsBatchResponse(List<CreateProductResponse> Products);

public interface ICreateProductsBatchUseCase
{
    Task<Result<CreateProductsBatchResponse, Error>> Execute(CreateProductsBatchRequest request);
}

internal sealed class CreateProductsBatchUseCase(
    ICreateProductUseCase createProductUseCase,
    IVectorDatabaseRepository vectorDatabaseRepository
) : ICreateProductsBatchUseCase
{
    public async Task<Result<CreateProductsBatchResponse, Error>> Execute(CreateProductsBatchRequest request)
    {
        if (request.Products == null || request.Products.Count == 0)
            return new BadRequestError("A lista de produtos não pode ser vazia.");

        await vectorDatabaseRepository.CreateCollectionIfNotExistsAsync(1536);

        var responses = new List<CreateProductResponse>();

        foreach (var productReq in request.Products)
        {
            var response = await createProductUseCase.Execute(productReq);
            if (response.IsSuccess)
                responses.Add(response.Value!);
        }

        return new CreateProductsBatchResponse(responses);
    }
}