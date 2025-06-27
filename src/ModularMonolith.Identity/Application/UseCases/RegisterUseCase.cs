using Microsoft.AspNetCore.Identity;
using ModularMonolith.Core.Utils;
using ModularMonolith.Identity.Domain;
using ModularMonolith.Identity.Domain.Interfaces;

namespace ModularMonolith.Identity.Application.UseCases;

public class RegisterUserRequest
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class RegisterUserResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public interface IRegisterUseCase
{
    Task<Result<RegisterUserResponse, Error>> ExecuteAsync(RegisterUserRequest request);
}

internal sealed class RegisterUseCase(IUserRepository userRepository) : IRegisterUseCase
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<Result<RegisterUserResponse, Error>> ExecuteAsync(RegisterUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
            return new BadRequestError("UserName is required");
        if (string.IsNullOrWhiteSpace(request.Email))
            return new BadRequestError("Email is required");
        if (string.IsNullOrWhiteSpace(request.Password))
            return new BadRequestError("Password is required");

        var user = new User(
            Guid.NewGuid(),
            request.UserName,
            request.Email,
            "" // será preenchido após o hash
        );

        // Hash seguro da senha
        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        user.ChangePassword(passwordHash);
        var defaultClaim = UserClaim.GetDefaultType();
        user.AddClaim(defaultClaim.Value);
        user.AddClaim("user");

        // Persistência do usuário omitida
        await userRepository.Add(user);

        return new RegisterUserResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }
}