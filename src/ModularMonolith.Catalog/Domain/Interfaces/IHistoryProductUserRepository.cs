namespace ModularMonolith.Catalog.Domain.Interfaces;

internal interface IHistoryProductUserRepository
{
    Task<HistoryProductUser?> GetByUserIdAsync(Guid id);
    Task AddItemInHistoryAsync(Guid idUser, string idProduct);
    Task AddAsync(HistoryProductUser userHistory);
}