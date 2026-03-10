namespace FinancialManager.Domain.Exceptions;

public class ValidationException : DomainException
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Ocorreram um ou mais erros de validação.")
    {
        Errors = errors;
    }
}
