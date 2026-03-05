public class TransactionRepository : ITransactionRepository
{
    private readonly FinancialManagerDbContext _context;

    public TransactionRepository(FinancialManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> transactions)
    {
        await _context.Transactions.AddRangeAsync(transactions);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }
}