namespace FinancialManager.Application.DTOs;

public record UpdateProfileRequest(
    string Name,
    string Email
);
