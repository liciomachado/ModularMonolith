using ModularMonolith.Core.Utils;

namespace ModularMonolith.Identity.Shared;

public interface IIdentityUserData
{
    Task<Result<UserDataSharedResponse, Error>> GetUserDataByIdAsync(Guid id);
}

public record UserDataSharedResponse();