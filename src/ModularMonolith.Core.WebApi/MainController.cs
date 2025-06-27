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
}
