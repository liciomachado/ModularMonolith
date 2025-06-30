using ModularMonolith.Catalog.Application.Services;
using ModularMonolith.Catalog.Domain.Interfaces;
using ModularMonolith.Core.Utils;

namespace ModularMonolith.Catalog.Application.UseCases;

public interface IGetByTextFilterUseCase
{
    Task<Result<ProductResponse[], Error>> Execute(string text, int topK = 5);
}

internal sealed class GetByTextFilterUseCase(IEmbeddingService embeddingService, IVectorDatabaseRepository vectorDatabaseRepository) : IGetByTextFilterUseCase
{
    public async Task<Result<ProductResponse[], Error>> Execute(string text, int topK = 5)
    {
        var embedding = await embeddingService.GenerateEmbeddingAsync(text);
        if (embedding.Count == 0)
            return new BadRequestError("Não foi possível gerar o embedding para o texto fornecido.");

        var searchResult = await vectorDatabaseRepository.SearchSimilarProductsAsync(Guid.NewGuid().ToString(), embedding.ToList(), [], topK);

        return searchResult.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        }).ToArray();
    }
}