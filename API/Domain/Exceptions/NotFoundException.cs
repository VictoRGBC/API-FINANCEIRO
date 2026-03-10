namespace FinancialManager.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, Guid id)
        : base($"{entityName} com ID '{id}' não foi encontrado(a).") { }

    public NotFoundException(string message)
        : base(message) { }
}
