using ModularMonolith.Catalog.Domain;
using ModularMonolith.Catalog.Domain.Interfaces;

namespace ModularMonolith.Catalog.Infraestructure.Data;

internal sealed class HistoryProductUserRepository : IHistoryProductUserRepository
{
    private readonly List<HistoryProductUser> _history = [];
    public Task<HistoryProductUser?> GetByUserIdAsync(Guid id)
    {
        return Task.FromResult(_history.FirstOrDefault(x => x.UserId == id));
    }

    public Task AddItemInHistoryAsync(Guid idUser, string idProduct)
    {
        var user = _history.FirstOrDefault(x => x.UserId == idUser);
        if (user is null)
        {
            user = new HistoryProductUser(idUser, idProduct);
            _history.Add(user);
            return Task.CompletedTask;
        }

        user.AddProduct(idProduct);
        return Task.CompletedTask;
    }

    public Task AddAsync(HistoryProductUser userHistory)
    {
        _history.Add(userHistory);
        return Task.CompletedTask;
    }
}