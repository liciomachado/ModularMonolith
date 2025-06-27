using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;

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
}
