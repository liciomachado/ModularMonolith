using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Identity.Application.UseCases;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ModularMonolith.Identity.Api;

//[Authorize(Policy = "UserOrAdmin")] exemplo de claim de autorização, se necessário
public class IdentityController : MainController
{
    [HttpPost("auth")]
    [SwaggerOperation(Summary = "Realiza Login")]
    [SwaggerResponse(200, "Sucesso", typeof(LoginUserResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] ILoginUseCase useCase, [FromBody] LoginUserRequest request)
        => ManageResponse(await useCase.ExecuteAsync(request));

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registra um novo usuario")]
    [SwaggerResponse(200, "Sucesso", typeof(RegisterUserResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add([FromServices] IRegisterUseCase useCase, [FromBody] RegisterUserRequest request)
        => ManageResponse(await useCase.ExecuteAsync(request));

    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Obtém os dados do usuário autenticado")]
    [SwaggerResponse(200, "Sucesso", typeof(UserDataResponse))]
    [SwaggerResponse(401, "Não autorizado", typeof(ErrorRequest))]
    public async Task<IActionResult> GetUserData([FromServices] IGetUserDataUseCase useCase)
    {
        // Extrai o id do usuário do token JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? User.FindFirst(ClaimTypes.Name)
                          ?? User.FindFirst("sub"); // "sub" é padrão JWT

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("Token JWT inválido ou sem identificador de usuário.");

        // Chama o use case passando o id extraído
        var result = await useCase.ExecuteAsync(userId);
        return ManageResponse(result);
    }

    //[HttpGet("all-options")]
    //public IActionResult Secret([FromServices] IOptions<IdentityOptions> options)
    //{
    //    return Ok(options.Value);
    //}

    //[HttpGet("environment")]
    //public IActionResult Secret([FromServices] IConfiguration configuration)
    //{
    //    return Ok(configuration["environment"]);
    //}

    //[HttpGet("default-appsettings")]
    //public IActionResult DefaultSettings([FromServices] IConfiguration configuration)
    //{
    //    return Ok(configuration["DefaultAppsettings"]);
    //}

    //[HttpGet("appsettings-env")]
    //public IActionResult SettingsEnv([FromServices] IConfiguration configuration)
    //{
    //    return Ok(configuration["EnvironmentSettings"]);
    //}
}