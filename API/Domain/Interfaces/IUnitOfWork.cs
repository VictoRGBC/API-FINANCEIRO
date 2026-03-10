namespace FinancialManager.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository AccountRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
