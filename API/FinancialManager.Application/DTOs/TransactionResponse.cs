namespace FinancialManager.Application.DTOs;

public record TransactionResponse(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    string Type,
    Guid AccountId,
    Guid CategoryId,
    DateTime CreatedAt
);
