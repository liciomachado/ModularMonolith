using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Catalog.Application.UseCases;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using Swashbuckle.AspNetCore.Annotations;

namespace ModularMonolith.Catalog.Api;

public class CatalogController : MainController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Recupera itens do catalogo")]
    [SwaggerResponse(200, "Sucesso", typeof(string))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] IGetProdutsUseCase useCase)
        => ManageResponse(await useCase.Execute());

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Recupera um produto pelo id")]
    [SwaggerResponse(200, "Sucesso", typeof(ProductDetailResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> GetById([FromServices] IGetProductByIdUseCase useCase, [FromRoute] string id)
        => ManageResponse(await useCase.Execute(id));

    [HttpPost]
    [SwaggerOperation(Summary = "Cadastra um novo produto")]
    [SwaggerResponse(201, "Produto criado com sucesso", typeof(CreateProductResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    public async Task<IActionResult> Create(
        [FromServices] ICreateProductUseCase useCase,
        [FromBody] CreateProductRequest request)
    {
        var result = await useCase.Execute(request);
        return ManageResponse(result);
    }

    [HttpPost("batch")]
    [SwaggerOperation(Summary = "Cadastra uma lista de produtos")]
    [SwaggerResponse(201, "Produtos criados com sucesso", typeof(CreateProductsBatchResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    public async Task<IActionResult> CreateBatch(
        [FromServices] ICreateProductsBatchUseCase useCase,
        [FromBody] CreateProductsBatchRequest request)
    {
        var result = await useCase.Execute(request);
        return ManageResponse(result);
    }

    [HttpPost("sync-vector-db")]
    [SwaggerOperation(Summary = "Sincroniza todos os produtos do MongoDB para o banco vetorial")]
    [SwaggerResponse(200, "Sincronização realizada", typeof(SyncAllProductsToVectorDbResponse))]
    public async Task<IActionResult> SyncAllProductsToVectorDb(
        [FromServices] ISyncAllProductsToVectorDbUseCase useCase)
    {
        var result = await useCase.Execute();
        return ManageResponse(result);
    }

    [HttpGet("{id}/similar")]
    [SwaggerOperation(Summary = "Recupera produtos similares ao produto de referência")]
    [SwaggerResponse(200, "Sucesso", typeof(List<ProductResponse>))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> GetSimilarProducts(
        [FromServices] IGetSimilarProductsUseCase useCase,
        [FromRoute] string id,
        [FromQuery] int topK = 5)
    {
        var user = GetOrCreateClientId();

        var result = await useCase.Execute(user, id, topK);
        return ManageResponse(result);
    }
}