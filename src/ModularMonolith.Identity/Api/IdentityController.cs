using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Application.UseCases;
using Swashbuckle.AspNetCore.Annotations;

namespace ModularMonolith.Identity.Api;

public class IdentityController : MainController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Realiza Login")]
    [SwaggerResponse(200, "Sucesso", typeof(LoginResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] ILoginUseCase useCase, [FromBody] LoginRequest request)
        => ManageResponse(await useCase.ExecuteAsync(request));
}