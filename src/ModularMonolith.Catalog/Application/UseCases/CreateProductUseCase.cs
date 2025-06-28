using ModularMonolith.Catalog.Application.Services;
using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public record CreateProductRequest(string Name, string Description, decimal Price, int Stock);
public record CreateProductResponse(string Id, string Name, string Description, decimal Price, int Stock);

public interface ICreateProductUseCase
{
    Task<Result<CreateProductResponse, Error>> Execute(CreateProductRequest request);
}

internal sealed class CreateProductUseCase(IProductRepository repository,
    IEmbeddingService embeddingService,
    IVectorDatabaseRepository vectorDatabaseRepository) : ICreateProductUseCase
{
    public async Task<Result<CreateProductResponse, Error>> Execute(CreateProductRequest request)
    {
        var validate = Validate(request);
        if (validate.IsFailure)
            return validate.Error!;

        var product = new Product(request.Name, request.Description, request.Price, request.Stock);

        var embedding = await embeddingService.GenerateEmbeddingAsync(product.ToTextEmbedding());
        product.AddEmbedding(embedding.ToList());

        // Supondo que o reposit�rio tenha um m�todo Add (adicione se necess�rio)
        await repository.Add(product);
        //await vectorDatabaseRepository.CreateCollectionIfNotExistsAsync(1536);
        await vectorDatabaseRepository.UpsertProductAsync(product);

        return new CreateProductResponse(product.Id, product.Name, product.Description, product.Price, product.Stock);
    }

    private static Result<bool, Error> Validate(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return new BadRequestError("Nome do produto n�o pode ser vazio.");
        if (string.IsNullOrWhiteSpace(request.Description))
            return new BadRequestError("Descri��o do produto n�o pode ser vazia.");

        if (request.Price <= 0)
            return new BadRequestError("Pre�o deve ser maior que zero.");

        if (request.Stock < 0)
            return new BadRequestError("Estoque n�o pode ser negativo.");

        return true;
    }
}