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

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Recupera um produto pelo id")]
    [SwaggerResponse(200, "Sucesso", typeof(ProductDetailResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> GetById([FromServices] IGetProductByIdUseCase useCase, [FromRoute] Guid id)
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
}