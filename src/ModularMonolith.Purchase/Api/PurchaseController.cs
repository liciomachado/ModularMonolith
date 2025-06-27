using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Purchase.Application.UseCases;
using Swashbuckle.AspNetCore.Annotations;

namespace ModularMonolith.Purchase.Api;

public class PurchaseController : MainController
{
    [HttpGet("cart/add-item")]
    [SwaggerOperation(Summary = "Recupera itens do catalogo")]
    [SwaggerResponse(200, "Sucesso", typeof(string))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] IAddItemCartUseCase useCase, AddItemCartRequest request)
    {
        return ManageResponse(await useCase.Execute(request));
    }
}