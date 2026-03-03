public class Category : Entity
{
    public string Name { get; private set; }
    public string Icon { get; private set; } // Opcional: para o Front-end
    public Guid UserId { get; private set; }

    // Auto-relacionamento
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    public Category(string name, Guid userId, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome da categoria é obrigatório.");

        Name = name;
        UserId = userId;
        ParentCategoryId = parentCategoryId;
    }
}