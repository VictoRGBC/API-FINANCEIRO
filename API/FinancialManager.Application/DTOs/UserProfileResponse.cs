namespace FinancialManager.Application.DTOs;

public record UserProfileResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive,
    int AccountsCount
);
