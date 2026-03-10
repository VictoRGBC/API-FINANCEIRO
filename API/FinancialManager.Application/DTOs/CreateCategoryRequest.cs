namespace FinancialManager.Application.DTOs;

public record CreateCategoryRequest(
    string Name,
    string Icon,
    Guid UserId,
    Guid? ParentCategoryId = null
);
