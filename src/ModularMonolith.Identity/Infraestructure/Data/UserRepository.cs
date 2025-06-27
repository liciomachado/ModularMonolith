using ModularMonolith.Identity.Domain;
using ModularMonolith.Identity.Domain.Interfaces;

namespace ModularMonolith.Identity.Infraestructure.Data;

internal sealed class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    private static readonly object _lock = new();

    public async Task<User?> GetByUserEmailAsync(string userNameOrEmail)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u =>
                u.Email.Equals(userNameOrEmail, StringComparison.OrdinalIgnoreCase));
        }
    }

    public async Task<User?> GetByUserIdAsync(Guid userId)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }
    }

    public Task Add(User user)
    {
        lock (_lock)
        {
            _users.Add(user);
        }

        return Task.CompletedTask;
    }

    public void Update(User user)
    {
        // Para testes em memória, nada a fazer pois User é referência
    }
}
