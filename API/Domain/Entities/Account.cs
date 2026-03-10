using FinancialManager.Domain.Common;

namespace FinancialManager.Domain.Entities;

public class Account : Entity
{
    public string Name { get; private set; }
    public string Type { get; private set; }
    public Guid UserId { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public Account(string name, string type, Guid userId)
    {
        Name = name;
        Type = type;
        UserId = userId;
    }

    public decimal GetBalance() => _transactions.Sum(t => t.Amount);
}