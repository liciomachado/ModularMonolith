using ModularMonolith.Core.Utils;

namespace ModularMonolith.Identity.Application.UseCases;

public interface ILoginUseCase
{
    Task<Result<LoginResponse, Error>> ExecuteAsync(LoginRequest request);
}

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token);

internal sealed class LoginUseCase : ILoginUseCase
{
    public async Task<Result<LoginResponse, Error>> ExecuteAsync(LoginRequest request)
    {
        return new LoginResponse("Bearer dshjkhdsahdsajhdsajhdasas");
    }
}