using FinancialManager.Domain.Entities;

namespace FinancialManager.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Account account);
    void Update(Account account);
}