public interface ITransactionRepository
{
    Task AddRangeAsync(IEnumerable<Transaction> transactions);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
}