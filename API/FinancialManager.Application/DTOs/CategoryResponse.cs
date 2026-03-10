namespace FinancialManager.Application.DTOs;

public record CategoryResponse(
    Guid Id,
    string Name,
    string Icon,
    Guid UserId,
    Guid? ParentCategoryId,
    DateTime CreatedAt
);
