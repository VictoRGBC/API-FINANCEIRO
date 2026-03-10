using FinancialManager.Domain.Entities;

namespace FinancialManager.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Category category);
    void Update(Category category);
}
