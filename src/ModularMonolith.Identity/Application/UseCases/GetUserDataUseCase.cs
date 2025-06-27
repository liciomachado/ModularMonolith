using ModularMonolith.Core.Utils;
using ModularMonolith.Identity.Domain.Interfaces;

namespace ModularMonolith.Identity.Application.UseCases;

public class UserDataResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedPassword { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<UserClaimDtoUserDataResponse> Claims { get; set; } = new();
}

public class UserClaimDtoUserDataResponse
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;
}
public interface IGetUserDataUseCase
{
    Task<Result<UserDataResponse?, Error>> ExecuteAsync(Guid userId);
}

internal sealed class GetUserDataUseCase(IUserRepository userRepository) : IGetUserDataUseCase
{
    public async Task<Result<UserDataResponse?, Error>> ExecuteAsync(Guid userId)
    {
        var user = await userRepository.GetByUserIdAsync(userId);
        if (user == null)
            return new NotFoundError("Usuario não encontrado");

        return new UserDataResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedPassword = user.UpdatedPassword,
            LastLoginAt = user.LastLoginAt,
            Claims = user.Claims.Select(c => new UserClaimDtoUserDataResponse { Type = c.Type, Value = c.Value }).ToList()
        };
    }
}