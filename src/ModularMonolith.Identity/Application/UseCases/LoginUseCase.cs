using Microsoft.AspNetCore.Identity;
using ModularMonolith.Core.Utils;
using ModularMonolith.Identity.Application.Services;
using ModularMonolith.Identity.Domain;
using ModularMonolith.Identity.Domain.Interfaces;

namespace ModularMonolith.Identity.Application.UseCases;

public interface ILoginUseCase
{
    Task<Result<LoginUserResponse, Error>> ExecuteAsync(LoginUserRequest request);
}

public class LoginUserRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class LoginUserResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Token { get; set; }
}

internal sealed class LoginUseCase(IUserRepository userRepository, GenerateJwtTokenService generateJwtTokenService) : ILoginUseCase
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<Result<LoginUserResponse, Error>> ExecuteAsync(LoginUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return new UnauthorizedError("UserName/Email and Password are required");

        var user = await userRepository.GetByUserEmailAsync(request.Email);
        if (user is not { IsActive: true })
            return new UnauthorizedError("Credenciais inválidas");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return new UnauthorizedError("Credenciais inválidas");


        user.UpdateLastLogin();
        userRepository.Update(user);

        var token = generateJwtTokenService.Execute(user);

        return new LoginUserResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = token
        };
    }
}