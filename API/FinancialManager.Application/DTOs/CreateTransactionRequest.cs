namespace FinancialManager.Application.DTOs;

public record CreateTransactionRequest(
    string Description,
    decimal Amount,
    DateTime Date,
    int Type,
    Guid AccountId,
    Guid CategoryId
);