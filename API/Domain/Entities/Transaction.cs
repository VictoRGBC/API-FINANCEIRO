using FinancialManager.Domain.Common;
using FinancialManager.Domain.Enums;
using FinancialManager.Domain.Exceptions;

namespace FinancialManager.Domain.Entities;

public class Transaction : Entity
{
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }

    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }

    public Transaction(string description, decimal amount, DateTime date, TransactionType type, Guid accountId, Guid categoryId)
    {
        if (amount == 0)
            throw new ValidationException("O valor da transação não pode ser zero.");

        Description = description;
        Amount = amount;
        Date = date;
        Type = type;
        AccountId = accountId;
        CategoryId = categoryId;
    }
}