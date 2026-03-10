using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Enums;
using FinancialManager.Domain.Exceptions;

namespace FinancialManager.Domain.Services;

public class TransferService
{
    public (Transaction Debit, Transaction Credit) CreateTransfer(
        Account sourceAccount,
        Account destinationAccount,
        decimal amount,
        string description,
        Guid categoryId)
    {
        if (amount <= 0)
            throw new ValidationException("O valor da transferência deve ser positivo.");

        if (sourceAccount.Id == destinationAccount.Id)
            throw new BusinessRuleException("As contas de origem e destino devem ser diferentes.");

        var debit = new Transaction(
            $"Transferência para {destinationAccount.Name}: {description}",
            -amount, DateTime.UtcNow, TransactionType.Expense, sourceAccount.Id, categoryId);

        var credit = new Transaction(
            $"Transferência de {sourceAccount.Name}: {description}",
            amount, DateTime.UtcNow, TransactionType.Revenue, destinationAccount.Id, categoryId);

        return (debit, credit);
    }
}