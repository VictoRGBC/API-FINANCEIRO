using FinancialManager.Domain.Entities;

namespace FinancialManager.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddRangeAsync(IEnumerable<Transaction> transactions);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
}