namespace FinancialManager.Application.DTOs;

public record AccountResponse(
    Guid Id,
    string Name,
    string Type,
    Guid UserId,
    decimal Balance,
    DateTime CreatedAt
);
