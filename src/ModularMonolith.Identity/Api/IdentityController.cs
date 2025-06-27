using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Application.Settings;
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

    [HttpGet("all-options")]
    public async Task<IActionResult> Secret([FromServices] IOptions<IdentityOptions> options)
    {
        return Ok(options.Value);
    }

    [HttpGet("environment")]
    public IActionResult Secret([FromServices] IConfiguration configuration)
    {
        return Ok(configuration["environment"]);
    }

    [HttpGet("default-appsettings")]
    public IActionResult DefaultSettings([FromServices] IConfiguration configuration)
    {
        return Ok(configuration["DefaultAppsettings"]);
    }

    [HttpGet("appsettings-env")]
    public IActionResult SettingsEnv([FromServices] IConfiguration configuration)
    {
        return Ok(configuration["EnvironmentSettings"]);
    }
}