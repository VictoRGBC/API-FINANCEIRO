namespace FinancialManager.Application.DTOs;

public record CreateAccountRequest(
    string Name,
    string Type,
    Guid UserId
);
