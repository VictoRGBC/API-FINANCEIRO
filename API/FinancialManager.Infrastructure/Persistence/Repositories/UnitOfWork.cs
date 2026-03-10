using FinancialManager.Domain.Interfaces;
using FinancialManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialManager.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FinancialManagerDbContext _context;
    private IDbContextTransaction? _transaction;

    public IAccountRepository AccountRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public ICategoryRepository CategoryRepository { get; }

    public UnitOfWork(
        FinancialManagerDbContext context,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository)
    {
        _context = context;
        AccountRepository = accountRepository;
        TransactionRepository = transactionRepository;
        CategoryRepository = categoryRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
