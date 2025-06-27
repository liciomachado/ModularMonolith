using ModularMonolith.Core.Utils;
using ModularMonolith.Identity.Shared;

namespace ModularMonolith.Identity.Infraestructure.SharedImplementations;

internal sealed class IdentityUserDataImplementation : IIdentityUserData
{
    public Task<Result<UserDataSharedResponse, Error>> GetUserDataByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}