namespace FinancialManager.Application.DTOs;

public record AuthResponse(
    string Token,
    string Email,
    string Name,
    DateTime ExpiresAt
);
