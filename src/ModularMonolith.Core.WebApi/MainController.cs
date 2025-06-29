using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using System.Security.Claims;

namespace ModularMonolith.Core.WebApi;

[Route("api/[controller]")]
[ApiController]
public abstract class MainController : ControllerBase
{
    protected ActionResult ManageError(Error error)
    {
        if (error is NotFoundError)
            return NotFound(new ErrorRequest(error.Message!));

        return BadRequest(new ErrorRequest(error.Message!));
    }

    protected ActionResult ManageResponse<T>(Result<T, Error> result)
    {
        return result.IsFailure
            ? ManageError(result.Error!)
            : Ok(result.Value!);
    }

    protected Guid? GetUserId()
    {
        // Extrai o id do usuário do token JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? User.FindFirst(ClaimTypes.Name)
                          ?? User.FindFirst("sub"); // "sub" é padrão JWT

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return null;

        return userId;
    }

    /// <summary>
    /// Obtém o identificador do usuário autenticado ou um identificador anônimo persistente (cookie/header).
    /// Se não existir, gera e retorna um novo identificador anônimo.
    /// </summary>
    protected Guid GetOrCreateClientId()
    {
        var userId = GetUserId();
        if (userId.HasValue)
            return userId.Value;

        // Tenta obter de header ou cookie
        string? anonId = Request.Headers["X-Client-Id"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(anonId))
            anonId = Request.Cookies["anon-client-id"];

        if (string.IsNullOrWhiteSpace(anonId) || !Guid.TryParse(anonId, out var clientId))
        {
            clientId = Guid.NewGuid();
            Response.Cookies.Append("anon-client-id", clientId.ToString(), new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
        }

        return clientId;
    }
}
