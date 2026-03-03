public class Transaction : Entity
{
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public TransactionType Type { get; private set; }

    // Relacionamentos (Foreign Keys de Domínio)
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }

    public Transaction(string description, decimal amount, DateTime date, TransactionType type, Guid accountId, Guid categoryId)
    {
        // Validação básica de Domínio (SRP)
        if (amount <= 0) throw new ArgumentException("O valor deve ser maior que zero.");

        Description = description;
        Amount = amount;
        Date = date;
        Type = type;
        AccountId = accountId;
        CategoryId = categoryId;
    }
}