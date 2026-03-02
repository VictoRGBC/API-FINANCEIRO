using System.Transactions;

public class Account : Entity
{
    public string Name { get; private set; }
    public string Type { get; private set; }
    public Guid UserId { get; private set; }

    // Coleção privada para garantir o encapsulamento (Princípio de OCP)
    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public Account(string name, string type, Guid userId)
    {
        // Aqui aplicaríamos Domain Validations (SRP)
        Name = name;
        Type = type;
        UserId = userId;
    }

    // O Saldo é calculado, não armazenado
    public decimal GetBalance() => _transactions.Sum(t => t.Amount);
}