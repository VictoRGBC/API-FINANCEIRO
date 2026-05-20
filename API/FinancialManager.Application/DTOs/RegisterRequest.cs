namespace FinancialManager.Application.DTOs;

public record RegisterRequest(
    string Name,
    string Email,
    string Password
);
