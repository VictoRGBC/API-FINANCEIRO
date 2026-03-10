namespace FinancialManager.Domain.Exceptions;

public class InsufficientBalanceException : DomainException
{
    public decimal CurrentBalance { get; }
    public decimal RequiredAmount { get; }

    public InsufficientBalanceException(decimal currentBalance, decimal requiredAmount)
        : base($"Saldo insuficiente. Saldo atual: {currentBalance:C}, Valor necessário: {requiredAmount:C}")
    {
        CurrentBalance = currentBalance;
        RequiredAmount = requiredAmount;
    }
}
