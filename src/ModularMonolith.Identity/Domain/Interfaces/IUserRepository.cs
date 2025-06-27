namespace ModularMonolith.Identity.Domain.Interfaces;

internal interface IUserRepository
{
    Task<User?> GetByUserEmailAsync(string userNameOrEmail);
    Task<User?> GetByUserIdAsync(Guid userId);
    void Update(User user);
    Task Add(User user);
}
