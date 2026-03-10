using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Interfaces;
using FinancialManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly FinancialManagerDbContext _context;

    public AccountRepository(FinancialManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public void Update(Account account)
    {
        _context.Accounts.Update(account);
    }
}
