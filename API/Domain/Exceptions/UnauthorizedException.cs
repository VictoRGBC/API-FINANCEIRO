namespace FinancialManager.Domain.Exceptions;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
