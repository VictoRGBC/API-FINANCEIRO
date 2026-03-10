using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Interfaces;
using FinancialManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FinancialManagerDbContext _context;

    public CategoryRepository(FinancialManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }
}
