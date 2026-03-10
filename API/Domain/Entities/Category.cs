using FinancialManager.Domain.Common;
using FinancialManager.Domain.Exceptions;

namespace FinancialManager.Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; }
    public string Icon { get; private set; }
    public Guid UserId { get; private set; }

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    public Category(string name, Guid userId, string icon = "💰", Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("O nome da categoria é obrigatório.");

        Name = name;
        UserId = userId;
        Icon = icon;
        ParentCategoryId = parentCategoryId;
    }
}