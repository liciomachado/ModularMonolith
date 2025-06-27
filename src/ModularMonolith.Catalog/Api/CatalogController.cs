using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.ExternalServices.Car;
using Swashbuckle.AspNetCore.Annotations;

namespace ModularMonolith.Catalog.Api;

public class CatalogController : MainController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Recupera itens do catalogo")]
    [SwaggerResponse(200, "Sucesso", typeof(string))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] ICarGateway carGateway)
    {
        var car = await carGateway.ConsultTerritoryByCode("1234");
        return ManageResponse(car);
    }
}